namespace FaceitSharp.Chat.Modules.Networking;

/// <summary>
/// Represents the source of a socket event
/// </summary>
public enum SocketEventSource
{
    /// <summary>
    /// Event happened due to an error
    /// </summary>
    Error,
    /// <summary>
    /// Event happened because the user initiated it
    /// </summary>
    User,
    /// <summary>
    /// Event happened because the server initiated it
    /// </summary>
    Server,
    /// <summary>
    /// Not sure why the event happened
    /// </summary>
    Unknown,
    /// <summary>
    /// Event happened because there was no activity for a certain time period
    /// </summary>
    NoActivity,
}
