namespace FaceitSharp.Chat.Network;

using XMPP.Stanzas;

public interface IChatSocket : IAsyncDisposable
{
    IObservable<string> MessageReceived { get; }

    IObservable<XmlElement> ElementReceived { get; }

    IObservable<XmlElement> ElementReceivedUnhandled { get; }

    IObservable<Stanza> StanzaReceived { get; }

    bool Connected { get; }

    bool Started { get; }

    Task<bool> Connect();

    Task Send(XmlElement element, bool instant = false);

    Task<Stanza> Expect(IResponseExpected expectation, double? timeoutSec = null);

    Task<Stanza> Send(IStanzaRequest request, double? timeoutSec = null, bool instant = false);
}

public class ChatSocket(
    ILogger<ChatSocket> _logger,
    IFaceitConfig _config) : Socket(_logger), IChatSocket
{
    public const string DEFAULT_URI = "wss://chat-server.faceit.com/websocket";
    public const int DEFAULT_KEEP_ALIVE = 35;
    public const int DEFAULT_RECONNECT = 35;
    public const int DEFAULT_RECONNECT_ERROR = 35;
    public const double DEFAULT_RESPONSE_TIMEOUT = 35;
    public const string DEFAULT_APP_VERSION = "2ebc5d5";
    public static Encoding DEFAULT_ENCODING = Encoding.UTF8;

    private readonly Subject<string> _receivedMessage = new();
    private readonly Subject<XmlElement> _receivedElement = new();
    private readonly Subject<XmlElement> _receivedElementUnhandled = new();
    private readonly Subject<Stanza> _receivedStanza = new();

    private readonly Type[] _globalStanzas = [typeof(Message), typeof(Presence), typeof(Iq), typeof(Features)];
    private readonly ConcurrentDictionary<IResponseExpected, TaskCompletionSource<Stanza>> _resolvers = [];

    public IObservable<string> MessageReceived => _receivedMessage.AsObservable();

    public IObservable<XmlElement> ElementReceived => _receivedElement.AsObservable();

    public IObservable<XmlElement> ElementReceivedUnhandled => _receivedElementUnhandled.AsObservable();

    public IObservable<Stanza> StanzaReceived => _receivedStanza.AsObservable();

    public override string Name => "FaceIT Chat";

    public Task<bool> Connect()
    {
        return Connect(_config.Chat);
    }

    public async Task Send(XmlElement element, bool instant = false)
    {
        await Send(element.ToXmlString(), instant);
    }

    public async Task<Stanza> Send(IStanzaRequest request, double? timeoutSec = null, bool instant = false)
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

        var timeout = timeoutSec.HasValue
            ? TimeSpan.FromSeconds(timeoutSec.Value)
            : TimeSpan.FromSeconds(_config.Chat.ReconnectTimeout);

        var ct = new CancellationTokenSource(timeout);
        ct.Token.Register(() =>
        {
            _resolvers.TryRemove(expectation, out _);
            tsc.TrySetCanceled();
        }, useSynchronizationContext: false);

        return await tsc.Task;
    }

    public void CheckResponses(XmlElement element)
    {
        var local = _resolvers.ToArray();
        var allTypes = local
            .SelectMany(t => t.Key.UniqueTypes)
            .Union(_globalStanzas)
            .Distinct()
            .ToArray();

        var stanza = element.GetStanza(allTypes);
        if (stanza is null)
        {
            _receivedElementUnhandled.OnNext(element);
            return;
        }

        _receivedStanza.OnNext(stanza);
        foreach (var (expectation, tsc) in local)
        {
            if (!expectation.Matches(stanza)) continue;

            _resolvers.TryRemove(expectation, out _);
            tsc.TrySetResult(stanza);
        }
    }

    public override Task OnMessageBinary(byte[] message)
    {
        var text = DEFAULT_ENCODING.GetString(message);
        return OnMessageText(text);
    }

    public override async Task OnMessageText(string message)
    {
        try
        {
            _receivedMessage.OnNext(message);
            var element = message.FromXmlString();
            if (element is null)
            {
                _logger.LogWarning("[XMPP::{name}] >> INVALID XML RECEIVED >> {message}", Name, message);
                return;
            }

            _receivedElement.OnNext(element);
            CheckResponses(element);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[XMPP::{name}] >> MESSAGE HANDLER ERROR", Name);
            await OnException("On message handler", ex);
            if (RethrowExceptions) throw;
        }
    }

    public override async ValueTask DisposeAsync()
    {
        foreach (var tsc in _resolvers.Values)
            tsc.TrySetCanceled();

        _resolvers.Clear();
        _receivedMessage.Dispose();
        _receivedElement.Dispose();
        _receivedElementUnhandled.Dispose();
        _receivedStanza.Dispose();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
