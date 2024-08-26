using System.Net.WebSockets;
using Websocket.Client;

namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Allows for configuring a socket connection
/// </summary>
public interface ISocketConfig
{
    /// <summary>
    /// Sets the URI to connect to
    /// </summary>
    /// <param name="uri">The URI to connect to</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithUri(string uri);

    /// <summary>
    /// Sets the protocol to use for the connection
    /// </summary>
    /// <param name="protocol">The protocol to use</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithProtocol(string protocol);

    /// <summary>
    /// Sets the reconnect timeout to use for the connection
    /// </summary>
    /// <param name="timeout">The number of seconds to wait</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithReconnectTimeout(TimeSpan timeout);

    /// <summary>
    /// Sets the reconnect timeout to use for the connection when an error occurs
    /// </summary>
    /// <param name="timeout">The number of seconds to wait</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithReconnectTimeoutError(TimeSpan timeout);

    /// <summary>
    /// Sets the interval to use for waiting between messages to keep the connection alive
    /// </summary>
    /// <param name="interval">The number of seconds to wait</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithKeepAliveInterval(TimeSpan interval);

    /// <summary>
    /// Configures the underlying web socket options
    /// </summary>
    /// <param name="config">The configuration action</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithConfig(Action<ClientWebSocketOptions> config);

    /// <summary>
    /// Sets the logger to use for the connection
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithLogger(ILogger<WebsocketClient> logger);

    /// <summary>
    /// Sets whether or not to automatically reconnect to the socket if disconnected
    /// </summary>
    /// <param name="enabled">Whether or not to attempt reconnects</param>
    /// <returns>The config builder for chaining</returns>
    ISocketConfig WithReconnects(bool enabled = true);

    /// <summary>
    /// Changes the encoding to use for binary data
    /// </summary>
    /// <param name="encoding">The encoding to use</param>
    /// <returns>The config builder for chaining</returns>
    /// <remarks>Default is <see cref="Encoding.UTF8"/></remarks>
    ISocketConfig WithEncoding(Encoding encoding);
}

internal class SocketConfig : ISocketConfig
{
    private Uri? _uri;
    private TimeSpan? _reconnectTimeout;
    private TimeSpan? _keepAliveInterval;
    private TimeSpan? _reconnectErrorTimeout;
    private string? _protocol;
    private readonly List<Action<ClientWebSocketOptions>> _webSocketConfig = [];
    private ILogger<WebsocketClient>? _logger;
    private bool _noReconnects = false;

    public Encoding? Encoder { get; set; }

    public ISocketConfig WithLogger(ILogger<WebsocketClient> logger)
    {
        _logger = logger;
        return this;
    }

    public ISocketConfig WithEncoding(Encoding encoding)
    {
        Encoder = encoding;
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

    public ISocketConfig WithReconnects(bool enabled = true)
    {
        _noReconnects = !enabled;
        return this;
    }

    public ISocketConfig WithReconnectTimeout(TimeSpan timeout)
    {
        _reconnectTimeout = timeout;
        return this;
    }

    public ISocketConfig WithReconnectTimeoutError(TimeSpan timeout)
    {
        _reconnectErrorTimeout = timeout;
        return this;
    }

    public ISocketConfig WithKeepAliveInterval(TimeSpan interval)
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
                    KeepAliveInterval = _keepAliveInterval ?? TimeSpan.FromSeconds(35),
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
            IsReconnectionEnabled = !_noReconnects,
            ReconnectTimeout = _reconnectTimeout,
            ErrorReconnectTimeout = _reconnectErrorTimeout,

        };
    }
}
