using System.Net.WebSockets;
using Websocket.Client;

namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Represents the publicly visible properties and events for socket connections
/// </summary>
public interface ISocketModule
{
    /// <summary>
    /// Indicates that the connection has been established
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Indicates that the socket will attempt to reconnect if the connection is lost
    /// </summary>
    bool ReconnectionEnabled { get; }

    /// <summary>
    /// Triggered when a connection event occurs
    /// </summary>
    IObservable<ISocketEventConnect> ConnectionEvents { get; }

    /// <summary>
    /// Triggered when the socket connection has been established
    /// </summary>
    IObservable<ISocketEventConnect> ConnectionEstablished { get; }

    /// <summary>
    /// Triggered when a reconnection event occurs
    /// </summary>
    IObservable<ISocketEventReconnect> ReconnectionEvents { get; }

    /// <summary>
    /// Triggered when the socket connection has been re-established
    /// </summary>
    IObservable<ISocketEventReconnect> ConnectionReestablished { get; }

    /// <summary>
    /// Triggered when the socket connection has been lost
    /// </summary>
    IObservable<ISocketEventDisconnect> Disconnected { get; }
}

internal abstract class SocketModule(
    ILogger _logger,
    IFaceitChatClient _client) : ChatModule(_logger, _client), ISocketModule
{
    private readonly Subject<SocketEvent> _socketEvents = new();
    private readonly Subject<string> _socketMessage = new();

    private WebsocketClient? _client;
    private Encoding _encoding = Encoding.UTF8;

    public IObservable<string> SocketMessages => _socketMessage.AsObservable();

    public IObservable<SocketEvent> SocketEvents => _socketEvents.AsObservable();

    public IObservable<ISocketEventConnect> ConnectionEvents
        => SocketEvents.Where(x =>
            x.Type == SocketEventType.ConnectionFailed ||
            x.Type == SocketEventType.Connecting ||
            x.Type == SocketEventType.Connected);

    public IObservable<ISocketEventConnect> ConnectionEstablished
        => ConnectionEvents.Where(x => x.Type == SocketEventType.Connected);

    public IObservable<ISocketEventReconnect> ReconnectionEvents
        => SocketEvents.Where(x =>
            x.Type == SocketEventType.Reconnecting ||
            x.Type == SocketEventType.Reconnected);

    public IObservable<ISocketEventReconnect> ConnectionReestablished
        => ReconnectionEvents.Where(x => x.Type == SocketEventType.Reconnected);

    public IObservable<ISocketEventDisconnect> Disconnected
        => SocketEvents.Where(x => x.Type == SocketEventType.Disconnected);

    public bool Connected => Started && (_client?.IsRunning ?? false);

    public bool Started => _client?.IsStarted ?? false;

    public bool ReconnectionEnabled => _client?.IsReconnectionEnabled ?? false;

    public virtual async Task<bool> Connect(Action<ISocketConfig> config)
    {
        _socketEvents.OnNext(new SocketEvent
        {
            Source = SocketEventSource.User,
            Type = SocketEventType.Connecting
        });

        var client = _client ?? CreateClient(config);

        Debug("Connecting to socket");
        try
        {
            await client.StartOrFail();
            Debug("Socket connected");
            return true;
        }
        catch (Exception ex)
        {
            Error(ex, "Error connecting to socket");
            _socketEvents.OnNext(new SocketEvent
            {
                Source = SocketEventSource.Error,
                Type = SocketEventType.ConnectionFailed,
            });
            return false;
        }
    }

    public virtual Task<bool> Connect(FaceitConfigSocket config, string protocol, bool reconnects)
    {
        return Connect(x => x
            .WithUri(config.Url)
            .WithEncoding(config.Encoding)
            .WithReconnectTimeout(config.Reconnect)
            .WithReconnectTimeoutError(config.ReconnectError)
            .WithKeepAliveInterval(config.KeepAlive)
            .WithProtocol(protocol)
            .WithReconnects(reconnects));
    }

    public WebsocketClient CreateClient(Action<ISocketConfig> config)
    {
        Box(() =>
        {
            var bob = new SocketConfig();
            config(bob);
            _client = bob.Create();
            _client.Name = ModuleName;
            _encoding = bob.Encoder ?? _encoding;
            _client.MessageEncoding = _encoding;

            Manage(_client
                .MessageReceived
                .Subscribe(HandleMessageReceived));

            Manage(_client
                .DisconnectionHappened
                .Subscribe(HandleDisconnection));

            Manage(_client
                .ReconnectionHappened
                .Subscribe(HandleReconnection));
        }, "Configuring Socket Client");

        if (_client is null)
            throw new InvalidOperationException("Failed to create client");

        return _client;
    }

    public async Task<WebsocketClient> GetClient()
    {
        if (_client is null)
            throw new InvalidOperationException("Socket not initialized. Did you forget to call `Connect(Action<ISocketConfig>)`?");

        if (Connected) return _client;

        await Box(_client.StartOrFail, "Starting socket");
        if (!Connected) throw new InvalidOperationException("Failed to start socket");
        return _client;
    }

    public virtual async Task Disconnect(string? reason = null, WebSocketCloseStatus? status = null)
    {
        if (_client is null) return;

        status ??= WebSocketCloseStatus.NormalClosure;
        reason ??= "No reason provided";

        await Box(() => _client.StopOrFail(status.Value, reason), 
            "Disconnecting: {status} - {reason}", status.Value, reason);

        if (_client is null) return;

        _client.Dispose();
        _client = null;
    }

    public virtual Task Send(string message, bool instant)
    {
        return Box(async () =>
        {
            var client = await GetClient();
            if (instant)
            {
                await client.SendInstant(message);
                return;
            }

            client.Send(message);
        }, "Sending Text All: {message}", message);
    }

    public virtual Task Send(byte[] message, bool instant)
    {
        return Box(async () =>
        {
            var client = await GetClient();
            if (instant)
            {
                await client.SendInstant(message);
                return;
            }

            client.Send(message);
        }, "Sending Binary All: {length}", message.Length);
    }

    public virtual void HandleMessageReceived(ResponseMessage message)
    {
        Box(() =>
        {
            if (message.MessageType == WebSocketMessageType.Text &&
                !string.IsNullOrEmpty(message.Text))
            {
                _socketMessage.OnNext(message.Text);
                return;
            }

            if (message.MessageType == WebSocketMessageType.Binary &&
                message.Binary is not null)
            {
                _socketMessage.OnNext(_encoding.GetString(message.Binary));
                return;
            }

            if (message.MessageType == WebSocketMessageType.Binary &&
                message.Stream is not null)
            {
                _socketMessage.OnNext(_encoding.GetString(message.Stream.ToArray()));
                return;
            }
        }, "SOCKET MESSAGE RECEIVE >> {type} >> {value}",
        message.MessageType.ToString(), message.Text ?? "Binary Data");
    }

    public virtual void HandleDisconnection(DisconnectionInfo info)
    {
        Box(() =>
        {
            var reconType = ReconnectionEnabled ? SocketEventType.Reconnecting : SocketEventType.Disconnected;
            var type = SocketEventType.Disconnected;
            var source = SocketEventSource.Unknown;

            switch(info.Type)
            {
                case DisconnectionType.Exit:
                    type = SocketEventType.Disconnected;
                    source = SocketEventSource.User;
                    break;
                case DisconnectionType.Lost:
                    type = reconType;
                    source = SocketEventSource.Server;
                    break;
                case DisconnectionType.NoMessageReceived:
                    type = reconType;
                    source = SocketEventSource.NoActivity;
                    break;
                case DisconnectionType.Error:
                    type = reconType;
                    source = SocketEventSource.Error;
                    break;
                case DisconnectionType.ByUser:
                    type = SocketEventType.Disconnected;
                    source = SocketEventSource.User;
                    break;
                case DisconnectionType.ByServer:
                    type = reconType;
                    source = SocketEventSource.Server;
                    break;
            }

            var evt = new SocketEvent 
            { 
                Type = type,
                Source = source,
                Error = info.Exception,
                Reason = info.CloseStatusDescription,
                Attempt = 0
            };
            _socketEvents.OnNext(evt);

            if (type != SocketEventType.Disconnected || _client is null)
                return;

            _client.Dispose();
            _client = null;
        }, "SOCKET DISCONNECTED >> {type} >> [{closeType}] {reason}", 
            info.Type.ToString(), 
            info.CloseStatus?.ToString() ?? "No status provided",
            info.CloseStatusDescription ?? "No reason provided");
    }

    public virtual void HandleReconnection(ReconnectionInfo info)
    {
        Box(() =>
        {
            var type = info.Type == ReconnectionType.Initial 
                ? SocketEventType.Connected 
                : SocketEventType.Reconnected;

            var source = info.Type switch
            {
                ReconnectionType.Initial => SocketEventSource.User,
                ReconnectionType.Lost => SocketEventSource.Server,
                ReconnectionType.NoMessageReceived => SocketEventSource.NoActivity,
                ReconnectionType.Error => SocketEventSource.Error,
                ReconnectionType.ByUser => SocketEventSource.User,
                ReconnectionType.ByServer => SocketEventSource.Server,
                _ => SocketEventSource.Unknown
            };

            var evt = new SocketEvent
            {
                Type = type,
                Source = source
            };
            _socketEvents.OnNext(evt);
        }, "SOCKET RECONNECTED >> {type}", info.Type.ToString());
    }

    public override async Task OnCleanup()
    {
        if (Started)
            await Disconnect("Disposing");

        await base.OnCleanup();
    }
}
