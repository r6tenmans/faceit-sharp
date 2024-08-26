namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Indicates the type of connect or reconnect event
/// </summary>
public enum SocketEventType
{
    /// <summary>
    /// The socket connect is attempting to connect for the first time
    /// </summary>
    Connecting,
    /// <summary>
    /// The socket connection was established for the first time
    /// </summary>
    Connected,
    /// <summary>
    /// The initial connection attempt failed
    /// </summary>
    ConnectionFailed,
    /// <summary>
    /// The socket connection was lost and is attempting to reconnect
    /// </summary>
    Reconnecting,
    /// <summary>
    /// The socket connection was re-established
    /// </summary>
    Reconnected,
    /// <summary>
    /// The socket connection was lost and is not attempting to reconnect
    /// </summary>
    Disconnected,
}
