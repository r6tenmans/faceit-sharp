namespace FaceitSharp.Chat;

/// <summary>
/// A collection of constant variables for use throughout the chat library
/// </summary>
public static class Constants
{
    /// <summary>
    /// The URI for the chat server
    /// </summary>
    public const string CHAT_URI = "wss://chat-server.faceit.com/websocket";

    /// <summary>
    /// The default duration to keep the chat server connection alive (seconds)
    /// </summary>
    public const double CHAT_KEEP_ALIVE = 35;

    /// <summary>
    /// The default timeout for waiting between reconnect attempts (seconds)
    /// </summary>
    public const double CHAT_RECONNECT = 35;

    /// <summary>
    /// The default timeout for waiting between reconnect attempts when an error occurs (seconds)
    /// </summary>
    public const double CHAT_RECONNECT_ERROR = 35;

    /// <summary>
    /// The default app version to use for the chat server
    /// </summary>
    public const string CHAT_APP_VERSION = "2ebc5d5";

    /// <summary>
    /// The protocol to use for the chat
    /// </summary>
    public const string CHAT_PROTOCOL = "xmpp";

    /// <summary>
    /// The number of seconds between pinging the server.
    /// </summary>
    /// <remarks>This should be below <see cref="CHAT_KEEP_ALIVE"/></remarks>
    public const double CHAT_PING_INTERVAL = 30;

    /// <summary>
    /// The number of seconds to wait for a response
    /// </summary>
    public const double CHAT_REQUEST_TIMEOUT = 3;

    /// <summary>
    /// The key for teams on the left side
    /// </summary>
    public const string CHAT_FACTION_LEFT = "faction1";

    /// <summary>
    /// The key for teams on the right side
    /// </summary>
    public const string CHAT_FACTION_RIGHT = "faction2";

    /// <summary>
    /// The default encoding to use for the chat
    /// </summary>
    public static Encoding CHAT_ENCODING { get; } = Encoding.UTF8;
}
