using System.Net.WebSockets;

namespace FaceitSharp.Chat;

public abstract class Socket(ILogger _logger) : IAsyncDisposable, IDisposable
{
    private WebsocketClient? _client;
    private readonly List<IDisposable> _subscriptions = [];
    private readonly bool _logEvents = false;

    public abstract string Name { get; }

    public virtual bool Debug
    {
        get
        {
#if DEBUG
            return _logEvents;
#else
            return false;
#endif
        }
    }

    public virtual bool RethrowExceptions { get; set; } = true;

    public bool Connected => Started && (_client?.IsRunning ?? false);

    public bool Started => _client?.IsStarted ?? false;

    public virtual async Task<WebsocketClient> GetClient()
    {
        if (_client is null)
            throw new InvalidOperationException("Socket not initialized. Did you forget to call `Init(Action<ISocketConfig>)`?");

        if (_client.IsStarted) return _client;

        await _client.StartOrFail();
        return _client;
    }

    public virtual async Task<bool> Connect(Action<ISocketConfig> config)
    {
        try
        {
            if (Debug) _logger.LogDebug("[SOCKET::{name}] >> CONFIGURING", Name);
            var bob = new SocketConfig();
            config(bob);
            _client = bob.Create();
            _client.Name = Name;

            _subscriptions.Add(_client
                .DisconnectionHappened
                .Subscribe(async t =>
                {
                    try
                    {
                        if (Debug) _logger.LogWarning("[SOCKET::{name}] >> DISCONNECTED", Name);
                        await OnDisconnected(t);
                        if (t.Exception is not null)
                            await OnException("Disconnection error", t.Exception);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "[SOCKET::{name}] >> DISCONNECTION HANDLER ERROR", Name);
                        await OnException("Disconnection handler error", ex);
                        if (RethrowExceptions) throw;
                    }
                }));

            _subscriptions.Add(_client
                .MessageReceived
                .Subscribe(async t =>
                {
                    try
                    {
                        if (Debug) _logger.LogDebug("[SOCKET::{name}] >> MESSAGE RECEIVED >> {type} >> {value}", 
                                Name, t.MessageType.ToString(), t.Text ?? "Binary Data");
                        await OnMessage(t);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "[SOCKET::{name}] >> MESSAGE HANDLER ERROR", Name);
                        await OnException("Message handler error", ex);
                        if (RethrowExceptions) throw;
                    }
                }));

            _subscriptions.Add(_client
                .ReconnectionHappened
                .Subscribe(async t =>
                {
                    try
                    {
                        if (Debug) _logger.LogWarning("[SOCKET::{name}] >> RECONNECTED >> {type}", Name, t.Type.ToString());
                        await OnReconnect(t);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "[SOCKET::{name}] >> RECONNECTION HANDLER ERROR", Name);
                        await OnException("Reconnection handler error", ex);
                        if (RethrowExceptions) throw;
                    }
                }));

            if (Debug) _logger.LogInformation("[SOCKET::{name}] >> CONNECTING", Name);
            await _client.StartOrFail();
            if (Debug) _logger.LogInformation("[SOCKET::{name}] >> CONNECTED", Name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SOCKET::{name}] >> CONNECTION ERROR", Name);
            await OnException("Connection error", ex);
            if (RethrowExceptions) throw;
            return false;
        }
    }

    public virtual Task<bool> Connect(IFaceitSocketConfig config)
    {
        return Connect(x => x
            .WithUri(config.Uri)
            .WithReconnectTimeout(config.ReconnectTimeout)
            .WithReconnectTimeoutError(config.ReconnectTimeoutError)
            .WithKeepAliveInterval(config.KeepAliveInterval)
            .WithProtocol("xmpp"));
    }

    public virtual async Task Disconnect(
        string? reason = null,
        WebSocketCloseStatus? status = null)
    {
        if (_client is null) return;

        if (Debug) _logger.LogInformation("[SOCKET::{name}] >> DISCONNECTING", Name);
        await _client.Stop(
            status ?? WebSocketCloseStatus.NormalClosure,
            reason ?? string.Empty);
    }

    public virtual async Task Send(string message, bool instant = false)
    {
        if (Debug) _logger.LogDebug("[SOCKET::{name}] >> SENDING TEXT >> {message}", Name, message);
        var client = await GetClient();

        if (instant)
        {
            await client.SendInstant(message);
            return;
        }

        client.Send(message);
    }

    public virtual async Task Send(byte[] message, bool instant = false)
    {
        if (Debug) _logger.LogDebug("[SOCKET::{name}] >> SENDING BINARY >> {message}", Name, message);
        var client = await GetClient();

        if (instant)
        {
            await client.SendInstant(message);
            return;
        }

        client.Send(message);
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().Wait();
        GC.SuppressFinalize(this);
    }

    public virtual async ValueTask DisposeAsync()
    {
        _subscriptions.ForEach(x => x.Dispose());
        _subscriptions.Clear();

        if (_client is null) return;

        await Disconnect("Disposing");
        _client.Dispose();
        GC.SuppressFinalize(this);
    }

    #region Event Handlers
    public virtual Task OnDisconnected(DisconnectionInfo action) => Task.CompletedTask;

    public virtual Task OnConnected() => Task.CompletedTask;

    public virtual Task OnReconnect(ReconnectionInfo info)
    {
        return info.Type switch
        {
            ReconnectionType.Initial => OnConnected(),
            ReconnectionType.Lost => OnReconnectDueToConnectionLoss(),
            ReconnectionType.NoMessageReceived => OnReconnectDueToNoMessage(),
            ReconnectionType.Error => OnReconnectDueToError(),
            ReconnectionType.ByUser => OnReconnectDueToUser(),
            ReconnectionType.ByServer => OnReconnectDueToServer(),
            _ => Task.CompletedTask
        };
    }

    public virtual Task OnReconnectDueToConnectionLoss() => Task.CompletedTask;

    public virtual Task OnReconnectDueToNoMessage() => Task.CompletedTask;

    public virtual Task OnReconnectDueToError() => Task.CompletedTask;

    public virtual Task OnReconnectDueToUser() => Task.CompletedTask;

    public virtual Task OnReconnectDueToServer() => Task.CompletedTask;

    public virtual Task OnMessage(ResponseMessage message)
    {
        if (message.MessageType == WebSocketMessageType.Text &&
            !string.IsNullOrEmpty(message.Text))
            return OnMessageText(message.Text);

        if (message.MessageType == WebSocketMessageType.Binary &&
            message.Binary is not null)
            return OnMessageBinary(message.Binary);

        if (message.MessageType == WebSocketMessageType.Binary &&
            message.Stream is not null)
            return OnMessageBinary(message.Stream.ToArray());

        return OnMessageUnknown(message);
    }

    public virtual Task OnMessageText(string message) => Task.CompletedTask;

    public virtual Task OnMessageBinary(byte[] message) => Task.CompletedTask;

    public virtual Task OnMessageUnknown(ResponseMessage message) => Task.CompletedTask;

    public virtual Task OnException(string message, Exception ex) => Task.CompletedTask;
    #endregion
}
