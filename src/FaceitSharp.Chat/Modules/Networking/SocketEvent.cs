namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Represents an event that happened with the socket connection
/// </summary>
public interface ISocketEvent
{
    /// <summary>
    /// The type of event that was triggered
    /// </summary>
    SocketEventType Type { get; }

    /// <summary>
    /// The source of the event
    /// </summary>
    SocketEventSource Source { get; }

    /// <summary>
    /// The optional error that occurred
    /// </summary>
    Exception? Error { get; }
}

/// <summary>
/// Represents an event related to establishing a connection
/// </summary>
/// <remarks>
/// Occurs when the <see cref="ISocketEvent.Type"/> is:
/// <see cref="SocketEventType.Connected"/> - Connection was established
/// <see cref="SocketEventType.Connecting"/> - Connection is attempting to be established
/// <see cref="SocketEventType.ConnectionFailed"/> - Connection failed to be established
/// </remarks>
public interface ISocketEventConnect : ISocketEvent
{
    /// <summary>
    /// The number of attempts made to establish a connection
    /// </summary>
    int Attempt { get; }
}

/// <summary>
/// Represents an event related to disconnecting from a connection
/// </summary>
/// <remarks>
/// Occurs when the <see cref="ISocketEvent.Type"/> is:
/// <see cref="SocketEventType.Disconnected"/> - Socket connection lost and not attempting to reconnect
/// </remarks>
public interface ISocketEventDisconnect : ISocketEvent
{
    /// <summary>
    /// The reason the event occurred
    /// </summary>
    string? Reason { get; }
}

/// <summary>
/// Represents an event related to reconnecting to a connection
/// </summary>
/// <remarks>
/// Occurs when the <see cref="ISocketEvent.Type"/> is:
/// <see cref="SocketEventType.Reconnecting"/> - Socket is attempting to reconnect
/// <see cref="SocketEventType.Reconnected"/> - Socket has reconnected
/// </remarks>
public interface ISocketEventReconnect : ISocketEventConnect
{
    
}

internal class SocketEvent : ISocketEventReconnect, ISocketEventDisconnect
{
    public required SocketEventType Type { get; init; }

    public required SocketEventSource Source { get; init; }

    public Exception? Error { get; init; } = null;

    public string? Reason { get; init; } = null;

    public int Attempt { get; init; } = 0;
}
