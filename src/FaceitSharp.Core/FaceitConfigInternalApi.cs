namespace FaceitSharp.Core;

/// <summary>
/// Settings related to the internal Faceit API
/// </summary>
public class FaceitConfigInternalApi()
{
    /// <summary>
    /// The default URL for internal API requests
    /// </summary>
    public const string DEFAULT_URL = "https://api.faceit.com";

    /// <summary>
    /// The default user agent for internal API requests
    /// </summary>
    public const string DEFAULT_USER_AGENT = "Faceit-Sharp/v1.0";

    /// <summary>
    /// The URL of the internal API
    /// </summary>
    public string Url { get; set; } = DEFAULT_URL;

    /// <summary>
    /// Whether or not to re-throw handled errors from the internal API
    /// </summary>
    public bool ThrowErrors { get; set; } = false;

    /// <summary>
    /// The factory for fetching tokens for the internal API
    /// </summary>
    public Func<Task<string>> Token { get; set; } = () => Task.FromResult(string.Empty);

    /// <summary>
    /// The factory for fetching the user agent for the internal API
    /// </summary>
    public Func<Task<string>> UserAgent { get; set; } = () => Task.FromResult(DEFAULT_USER_AGENT);

    /// <summary>
    /// Settings related to the internal Faceit API
    /// </summary>
    /// <param name="section">The configuration section to draw from</param>
    public FaceitConfigInternalApi(IConfigurationSection section) : this()
    {
        Url = section.GetValue(nameof(Url), Url) ?? DEFAULT_URL;
        ThrowErrors = section.GetValue(nameof(ThrowErrors), ThrowErrors);
        Token = () => Task.FromResult(section.GetValue(nameof(Token), string.Empty) ?? string.Empty);
        UserAgent = () => Task.FromResult(section.GetValue(nameof(UserAgent), DEFAULT_USER_AGENT) ?? DEFAULT_USER_AGENT);
    }
}
