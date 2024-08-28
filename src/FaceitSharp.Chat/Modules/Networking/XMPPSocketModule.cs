namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Represents publicly visible properties and events for XMPP socket connections
/// </summary>
public interface IXMPPSocketModule : ISocketModule
{
    /// <summary>
    /// Triggered when a stanza is received but it couldn't be parsed
    /// </summary>
    IObservable<XmlElement> UnparsedElements { get; }

    /// <summary>
    /// Triggered when a stanza is received that isn't handled
    /// </summary>
    IObservable<Stanza> Stanzas { get; }

    /// <summary>
    /// Triggered when a message is received that couldn't be parsed
    /// </summary>
    IObservable<string> UnparsedMessages { get; }

    /// <summary>
    /// Sends the given XML element to the XMPP socket
    /// </summary>
    /// <param name="element">The XML element to send</param>
    /// <param name="instant">Whether to send the message instantly or queue it</param>
    Task Send(XmlElement element, bool instant);

    /// <summary>
    /// Sends the given request to the XMPP socket and awaits it's response
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="instant">Whether to send the message instantly or queue it</param>
    /// <param name="timeoutSec">The number of seconds to wait before timing out the response request</param>
    /// <returns>The stanza that was received in response to the sent message</returns>
    Task<Stanza> Send(IStanzaRequest request, bool instant = false, double? timeoutSec = null);

    /// <summary>
    /// Indicates that a specific response is expected
    /// </summary>
    /// <param name="expectation">The expectation</param>
    /// <param name="timeoutSec">The number of seconds to wait before timing out</param>
    /// <returns>The stanza that was received in response to the sent message</returns>
    Task<Stanza> Expect(IResponseExpected expectation, double? timeoutSec = null);
}

internal class XMPPSocketModule(
    ILogger _logger,
    IFaceitChatClient _client) : SocketModule(_logger, _client), IXMPPSocketModule
{
    private readonly Subject<Stanza> _parsedStanzas = new();
    private readonly Subject<XmlElement> _unparsedStanzas = new();
    private readonly Subject<string> _unparsedMessages = new();

    private IObservable<Stanza>? _parsedStanzasInstance;
    private IObservable<XmlElement>? _unparsedStanzaInstance;
    private IObservable<string>? _unparsedMessageInstance;

    private readonly Type[] _globalStanzas = [typeof(Message), typeof(Presence), typeof(Iq), typeof(Features)];
    private readonly ConcurrentDictionary<IResponseExpected, TaskCompletionSource<Stanza>> _resolvers = [];

    public IObservable<string> UnparsedMessages => _unparsedMessageInstance ??= _unparsedMessages.AsObservable();

    public IObservable<XmlElement> UnparsedElements => _unparsedStanzaInstance ??= _unparsedStanzas.AsObservable();

    public IObservable<Stanza> Stanzas => _parsedStanzasInstance ??= _parsedStanzas.AsObservable();

    public override string ModuleName => "XMPP Socket Module";

    public Task Send(XmlElement element, bool instant)
    {
        return Send(element.ToXmlString(), instant);
    }

    public async Task<Stanza> Send(IStanzaRequest request, bool instant = false, double? timeoutSec = null)
    {
        var expectation = new ResponseExpected();
        request.Expects(expectation);
        var task = Expect(expectation, timeoutSec);
        await Send(request.Serialize(), instant);
        return await task;
    }

    public async Task<Stanza> Expect(IResponseExpected expectation, double? timeoutSec = null)
    {
        var tsc = new TaskCompletionSource<Stanza>();
        _resolvers.TryAdd(expectation, tsc);
        var timeout = timeoutSec is null 
            ? Config.Chat.RequestTimeout
            : TimeSpan.FromSeconds(timeoutSec.Value);

        var ct = new CancellationTokenSource(timeout);
        ct.Token.Register(() =>
        {
            _resolvers.TryRemove(expectation, out _);
            tsc.TrySetCanceled();
        }, useSynchronizationContext: false);
        return await tsc.Task;
    }

    public Task<bool> Connect()
    {
        return Connect(Config.Chat, Config.Chat.Protocol, true);
    }

    public override Task OnSetup()
    {
        Manage(SocketMessages
            .Subscribe(t =>
            {
                Debug("SOCKET MESSAGE RECEIVED >> {data}", t);
                var element = MessageParser(t);
                if (element is null)
                {
                    _unparsedMessages.OnNext(t);
                    return;
                }

                var stanza = StanzaParser(element);
                if (stanza is null)
                {
                    _unparsedStanzas.OnNext(element);
                    return;
                }

                if (!StanzaHandler(stanza))
                    _parsedStanzas.OnNext(stanza);
            }));
        Manage(SocketMessagesSent
            .Subscribe(t => Debug("SOCKET MESSAGE SENT >> {data}", t)));
        return base.OnSetup();
    }

    public override Task OnCleanup()
    {
        foreach (var (_, tsc) in _resolvers)
            tsc.TrySetCanceled();
        _resolvers.Clear();
        _parsedStanzas.Dispose();
        _unparsedStanzas.Dispose();
        _unparsedMessages.Dispose();
        _parsedStanzasInstance = null;
        _unparsedStanzaInstance = null;
        _unparsedMessageInstance = null;
        return base.OnCleanup();
    }

    public bool StanzaHandler(Stanza stanza)
    {
        var handled = false;
        Box(() =>
        {
            var local = _resolvers.ToArray();
            foreach (var (expectation, tsc) in local)
            {
                if (!expectation.Matches(stanza)) continue;

                handled = true;
                _resolvers.TryRemove(expectation, out _);
                tsc.TrySetResult(stanza);
            }

        }, "STANZA RECEIVED >> RESPONSE PARSER >> {name}", stanza.GetType().Name);
        return handled;
    }

    public Stanza? StanzaParser(XmlElement element)
    {
        Stanza? output = null;
        Box(() =>
        {
            var allTypes = _resolvers
                .SelectMany(t => t.Key.UniqueTypes)
                .Union(_globalStanzas)
                .Distinct()
                .ToArray();

            output = element.GetStanza(allTypes);
            if (output is null)
                Debug("ELEMENT RECEIVED >> TO STANZA >> NO MATCHING PARSER >> {element}", element.ToXmlString());
        }, "ELEMENT RECEIVED >> TO STANZA >> {name}", element.Name);
        return output;
    }

    public XmlElement? MessageParser(string message)
    {
        XmlElement? output = null;
        Box(() =>
        {
            output = message.FromXmlString();
            if (output is null)
                Warning("MESSAGE RECEIVED >> TO ELEMENT >> FAILED TO PARSE MESSAGE >> {message}", message);
        }, "MESSAGE RECEIVED >> TO ELEMENT >> {message}", message);
        return output;
    }
}

internal record class StanzaParsed(Stanza? Stanza, XmlElement Element);