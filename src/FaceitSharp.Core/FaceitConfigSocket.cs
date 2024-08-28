namespace FaceitSharp.Core;

/// <summary>
/// Settings related to a Faceit socket
/// </summary>
public abstract class FaceitConfigSocket()
{
    /// <summary>
    /// The default keep alive interval for the chat server
    /// </summary>
    public const double DEFAULT_KEEP_ALIVE_SECONDS = 35;

    /// <summary>
    /// The default number of seconds to wait before reconnecting to the chat server
    /// </summary>
    public const double DEFAULT_RECONNECT_SECONDS = 35;

    /// <summary>
    /// The default number of seconds to wait before reconnecting to the chat server after an error
    /// </summary>
    public const double DEFAULT_RECONNECT_ERROR_SECONDS = 35;

    /// <summary>
    /// The default number of seconds between pinging the server
    /// </summary>
    public const double DEFAULT_PING_INTERVAL_SECONDS = 30;

    /// <summary>
    /// The number of seconds to wait for a response from the server
    /// </summary>
    public const double DEFAULT_REQUEST_TIMEOUT_SECONDS = 3;

    /// <summary>
    /// The default encoding to use for the chat
    /// </summary>
    public static Encoding DEFAULT_ENCODING { get; } = Encoding.UTF8;

    /// <summary>
    /// The default log level to use for logging
    /// </summary>
    public static LogLevel DEFAULT_LOG_LEVEL { get; } = LogLevel.Information;

    /// <summary>
    /// The Web Socket URI for the chat server
    /// </summary>
    public abstract string Url { get; set; }

    /// <summary>
    /// How long to wait for a message from the chat server before timing out
    /// </summary>
    public virtual TimeSpan KeepAlive { get; set; } = TimeSpan.FromSeconds(DEFAULT_KEEP_ALIVE_SECONDS);

    /// <summary>
    /// How long to wait before reconnecting to the chat server after a disconnect
    /// </summary>
    public virtual TimeSpan Reconnect { get; set; } = TimeSpan.FromSeconds(DEFAULT_RECONNECT_SECONDS);

    /// <summary>
    /// How long to wait before reconnecting to the chat server after an error occurred
    /// </summary>
    public virtual TimeSpan ReconnectError { get; set; } = TimeSpan.FromSeconds(DEFAULT_RECONNECT_ERROR_SECONDS);

    /// <summary>
    /// How long to wait between pinging the server
    /// </summary>
    public virtual TimeSpan PingInterval { get; set; } = TimeSpan.FromSeconds(DEFAULT_PING_INTERVAL_SECONDS);

    /// <summary>
    /// How long to wait for a response from the server
    /// </summary>
    public virtual TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(DEFAULT_REQUEST_TIMEOUT_SECONDS);

    /// <summary>
    /// The log level to use for logging
    /// </summary>
    public virtual LogLevel LogLevel { get; set; } = DEFAULT_LOG_LEVEL;

    /// <summary>
    /// The encoding to use for the web-socket connection
    /// </summary>
    public virtual Encoding Encoding { get; set; } = DEFAULT_ENCODING;

    /// <summary>
    /// Settings related to a Faceit socket
    /// </summary>
    /// <param name="section">The configuration section to draw from</param>
    public FaceitConfigSocket(IConfigurationSection section) : this()
    {
        Url = section.GetValue(nameof(Url), Url) ?? Url;
        KeepAlive = TimeSpan.FromSeconds(section.GetValue(nameof(KeepAlive), DEFAULT_KEEP_ALIVE_SECONDS));
        Reconnect = TimeSpan.FromSeconds(section.GetValue(nameof(Reconnect), DEFAULT_RECONNECT_SECONDS));
        ReconnectError = TimeSpan.FromSeconds(section.GetValue(nameof(ReconnectError), DEFAULT_RECONNECT_ERROR_SECONDS));
        PingInterval = TimeSpan.FromSeconds(section.GetValue(nameof(PingInterval), DEFAULT_PING_INTERVAL_SECONDS));
        RequestTimeout = TimeSpan.FromSeconds(section.GetValue(nameof(RequestTimeout), DEFAULT_REQUEST_TIMEOUT_SECONDS));

        var level = section.GetValue(nameof(LogLevel), DEFAULT_LOG_LEVEL.ToString());
        if (!Enum.TryParse<LogLevel>(level, true, out var logLevel))
            logLevel = DEFAULT_LOG_LEVEL;
        LogLevel = logLevel;
    }
}
