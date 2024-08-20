namespace FaceitSharp;

/// <summary>
/// A static instance for <see cref="IFaceitConfig"/>
/// </summary>
/// <param name="ApiToken">The API token for the internal API</param>
/// <param name="UserAgent">The user-agent to use for the internal API</param>
/// <param name="LogWebHooks">Whether or not to log webhook events</param>
/// <param name="InternalApiErrors">Whether or not to throw an exception on a failed API request</param>
/// <param name="InternalApiUrl">The URL to use for connecting to the FaceIT API</param>
public record class StaticFaceitConfig(
    string ApiToken,
    string UserAgent,
    bool LogWebHooks = true,
    bool InternalApiErrors = true,
    string InternalApiUrl = "https://api.faceit.com") : IFaceitConfig
{
    /// <summary>
    /// Resolves the <see cref="ApiToken"/>
    /// </summary>
    public Task<string> InternalApiToken() => Task.FromResult(ApiToken);

    /// <summary>
    /// Resolves the <see cref="UserAgent"/>
    /// </summary>
    public Task<string> InternalUserAgent() => Task.FromResult(UserAgent);
}
