namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Represents publicly visible properties and events for XMPP socket connections
/// </summary>
public interface IXMPPSocketModule : ISocketModule
{
    /// <summary>
    /// Triggered when an XML element is received from the XMPP socket
    /// </summary>
    IObservable<XmlElement> Elements { get; }

    /// <summary>
    /// Triggered when a stanza is received but it couldn't be parsed
    /// </summary>
    IObservable<XmlElement> UnparsedElements { get; }

    /// <summary>
    /// Triggered when a stanza is received, regardless of whether if it's a response or not
    /// </summary>
    IObservable<Stanza> AllStanzas { get; }

    /// <summary>
    /// Triggered when a stanza is received that isn't a response
    /// </summary>
    IObservable<Stanza> Stanzas { get; }

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
    private IObservable<XmlElement>? _elements;
    private IObservable<StanzaParsed>? _stanzaParsed;
    private IObservable<XmlElement>? _unparsedElements;
    private IObservable<Stanza>? _allStanzas;
    private IObservable<Stanza>? _stanzas;

    private readonly Type[] _globalStanzas = [typeof(Message), typeof(Presence), typeof(Iq), typeof(Features)];
    private readonly ConcurrentDictionary<IResponseExpected, TaskCompletionSource<Stanza>> _resolvers = [];

    public IObservable<XmlElement> Elements => _elements ??= SocketMessages
        .Select(MessageParser)
        .Where(t => t is not null)
        .Select(t => t!);

    public IObservable<StanzaParsed> StanzaParsed => _stanzaParsed ??= Elements
        .Select(StanzaParser);

    public IObservable<XmlElement> UnparsedElements => _unparsedElements ??= StanzaParsed
        .Where(t => t.Stanza is null)
        .Select(t => t.Element);

    public IObservable<Stanza> AllStanzas => _allStanzas ??= StanzaParsed
        .Where(t => t.Stanza is not null)
        .Select(t => t.Stanza!);

    public IObservable<Stanza> Stanzas => _stanzas ??= AllStanzas
        .Where(t => !StanzaHandler(t));

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
        Manage(Stanzas.Subscribe());
        return base.OnSetup();
    }

    public override Task OnCleanup()
    {
        foreach (var (_, tsc) in _resolvers)
            tsc.TrySetCanceled();
        _resolvers.Clear();
        _elements = null;
        _stanzaParsed = null;
        _unparsedElements = null;
        _allStanzas = null;
        _stanzas = null;
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

    public StanzaParsed StanzaParser(XmlElement element)
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
        return new (output, element);
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