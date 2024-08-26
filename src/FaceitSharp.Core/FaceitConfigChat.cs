namespace FaceitSharp.Core;

/// <summary>
/// Settings related to the FaceIT chat
/// </summary>
public class FaceitConfigChat : FaceitConfigSocket
{
    #region Defaults
    /// <summary>
    /// The default URL for the chat server
    /// </summary>
    public const string DEFAULT_URI = "wss://chat-server.faceit.com/websocket";

    /// <summary>
    /// The default protocol to use for the chat
    /// </summary>
    public const string DEFAULT_PROTOCOL = "xmpp";

    /// <summary>
    /// The default app version to use for the chat server
    /// </summary>
    public const string DEFAULT_APP_VERSION = "2ebc5d5";

    /// <summary>
    /// The default key for the teams on the left side
    /// </summary>
    public const string DEFAULT_FACTION_LEFT = "faction1";

    /// <summary>
    /// The default key for the teams on the right side
    /// </summary>
    public const string DEFAULT_FACTION_RIGHT = "faction2";
    #endregion

    #region Settings
    /// <summary>
    /// The Web Socket URI for the chat server
    /// </summary>
    public override string Url { get; set; } = DEFAULT_URI;

    /// <summary>
    /// The protocol to use for the web-socket connection
    /// </summary>
    public string Protocol { get; set; } = DEFAULT_PROTOCOL;

    /// <summary>
    /// The app version to use for the chat server
    /// </summary>
    public string AppVersion { get; set; } = DEFAULT_APP_VERSION;

    /// <summary>
    /// The key for the team on the left side
    /// </summary>
    public string FactionLeft { get; set; } = DEFAULT_FACTION_LEFT;

    /// <summary>
    /// The key for the team on the right side
    /// </summary>
    public string FactionRight { get; set; } = DEFAULT_FACTION_RIGHT;
    #endregion

    /// <summary>
    /// Settings related to the FaceIT chat
    /// </summary>
    public FaceitConfigChat() : base() { }

    /// <summary>
    /// Settings related to the FaceIT chat
    /// </summary>
    /// <param name="section">The configuration section to draw from</param>
    public FaceitConfigChat(IConfigurationSection section) : base(section)
    {
        Protocol = section.GetValue(nameof(Protocol), DEFAULT_PROTOCOL) ?? DEFAULT_PROTOCOL;
        AppVersion = section.GetValue(nameof(AppVersion), DEFAULT_APP_VERSION) ?? DEFAULT_APP_VERSION;
        FactionLeft = section.GetValue(nameof(FactionLeft), DEFAULT_FACTION_LEFT) ?? DEFAULT_FACTION_LEFT;
        FactionRight = section.GetValue(nameof(FactionRight), DEFAULT_FACTION_RIGHT) ?? DEFAULT_FACTION_RIGHT;
    }
}
