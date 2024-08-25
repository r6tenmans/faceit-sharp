using System.Net.WebSockets;

namespace FaceitSharp.Chat.Network;

public interface ISocketConfig
{
    ISocketConfig WithUri(string uri);

    ISocketConfig WithProtocol(string protocol);

    ISocketConfig WithReconnectTimeout(int timeout);

    ISocketConfig WithReconnectTimeoutError(int timeout);

    ISocketConfig WithKeepAliveInterval(int interval);

    ISocketConfig WithConfig(Action<ClientWebSocketOptions> config);

    ISocketConfig WithLogger(ILogger<WebsocketClient> logger);
}

internal class SocketConfig : ISocketConfig
{
    private Uri? _uri;
    private int _reconnectTimeout = 35;
    private int _keepAliveInterval = 35;
    private int _reconnectErrorTimeout = 35;
    private string? _protocol;
    private readonly List<Action<ClientWebSocketOptions>> _webSocketConfig = [];
    private ILogger<WebsocketClient>? _logger;

    public ISocketConfig WithLogger(ILogger<WebsocketClient> logger)
    {
        _logger = logger;
        return this;
    }

    public ISocketConfig WithUri(string uri)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri? result))
            throw new ArgumentException("Invalid URI: " + uri, nameof(uri));

        _uri = result;
        return this;
    }

    public ISocketConfig WithProtocol(string protocol)
    {
        _protocol = protocol;
        return this;
    }

    public ISocketConfig WithReconnectTimeout(int timeout)
    {
        _reconnectTimeout = timeout;
        return this;
    }

    public ISocketConfig WithReconnectTimeoutError(int timeout)
    {
        _reconnectErrorTimeout = timeout;
        return this;
    }

    public ISocketConfig WithKeepAliveInterval(int interval)
    {
        _keepAliveInterval = interval;
        return this;
    }

    public ISocketConfig WithConfig(Action<ClientWebSocketOptions> config)
    {
        _webSocketConfig.Add(config);
        return this;
    }

    public WebsocketClient Create()
    {
        if (_uri is null) throw new InvalidOperationException("URI must be set before creating the socket");

        ClientWebSocket ClientFactory()
        {
            var client = new ClientWebSocket
            {
                Options =
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(_keepAliveInterval),
                },
            };
            if (!string.IsNullOrEmpty(_protocol))
                client.Options.AddSubProtocol(_protocol);
            foreach (var action in _webSocketConfig)
                action?.Invoke(client.Options);
            return client;
        }

        return new WebsocketClient(_uri, _logger, ClientFactory)
        {
            ReconnectTimeout =
                _reconnectTimeout <= 0
                    ? null
                    : TimeSpan.FromSeconds(_reconnectTimeout),
            ErrorReconnectTimeout =
                _reconnectErrorTimeout <= 0
                    ? null
                    : TimeSpan.FromSeconds(_reconnectErrorTimeout),

        };
    }
}
