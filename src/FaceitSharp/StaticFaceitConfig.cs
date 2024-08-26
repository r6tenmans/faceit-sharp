namespace FaceitSharp;

using Chat;

/// <summary>
/// A static instance for <see cref="IFaceitConfig"/>
/// </summary>
/// <param name="ApiToken">The API token for the internal API</param>
/// <param name="UserAgent">The user-agent to use for the internal API</param>
/// <param name="LogWebHooks">Whether or not to log webhook events</param>
/// <param name="InternalApiErrors">Whether or not to throw an exception on a failed API request</param>
/// <param name="InternalApiUrl">The URL to use for connecting to the FaceIT API</param>
/// <param name="ChatUrl">The URL to the FaceIT chat server</param>
/// <param name="EdgeUrl">The URL to the FaceIT edge server</param>
/// <param name="KeepAliveSeconds">How long to await between messages for timing out reconnects</param>
/// <param name="ReconnectErrorSeconds">The number of seconds to wait between errored reconnect attempts</param>
/// <param name="ReconnectSeconds">The number of seconds to wait between reconnect attempts</param>
/// <param name="ResponseTimeoutSeconds">The number of seconds to wait before timing out a response request</param>
/// <param name="AppVersion">The version of the application</param>
public record class StaticFaceitConfig(
    string ApiToken,
    string UserAgent,
    bool LogWebHooks = true,
    bool InternalApiErrors = true,
    string InternalApiUrl = "https://api.faceit.com",
    string ChatUrl = Constants.CHAT_URI,
    string EdgeUrl = Constants.CHAT_URI,
    double ReconnectSeconds = Constants.CHAT_RECONNECT,
    double ReconnectErrorSeconds = Constants.CHAT_RECONNECT_ERROR,
    double KeepAliveSeconds = Constants.CHAT_KEEP_ALIVE,
    double ResponseTimeoutSeconds = Constants.CHAT_REQUEST_TIMEOUT,
    string AppVersion = Constants.CHAT_APP_VERSION) : IFaceitConfig
{
    /// <summary>
    /// Resolves the <see cref="ApiToken"/>
    /// </summary>
    public Task<string> InternalApiToken() => Task.FromResult(ApiToken);

    /// <summary>
    /// Resolves the <see cref="UserAgent"/>
    /// </summary>
    public Task<string> InternalUserAgent() => Task.FromResult(UserAgent);

    /// <summary>
    /// The connection settings for the edge WebSocket server
    /// </summary>
    public IFaceitSocketConfig Edge => new StaticFaceitSocketConfig(
        EdgeUrl, ReconnectSeconds, ReconnectErrorSeconds, KeepAliveSeconds, ResponseTimeoutSeconds, AppVersion);

    /// <summary>
    /// The connection settings for the chat WebSocket server
    /// </summary>
    public IFaceitSocketConfig Chat => new StaticFaceitSocketConfig(
        ChatUrl, ReconnectSeconds, ReconnectErrorSeconds, KeepAliveSeconds, ResponseTimeoutSeconds, AppVersion);
}

internal record class StaticFaceitSocketConfig(
    string Uri,
    double ReconnectTimeout,
    double ReconnectTimeoutError,
    double KeepAliveInterval,
    double ResponseTimeout,
    string AppVersion) : IFaceitSocketConfig { }