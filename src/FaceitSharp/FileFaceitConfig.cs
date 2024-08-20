
namespace FaceitSharp;

internal record class FileFaceitConfigConfig(
    string Section);

internal class FileFaceitConfig(
    IConfiguration _config,
    FileFaceitConfigConfig _static) : IFaceitConfig
{
    public string InternalApiUrl => Setting(nameof(InternalApiUrl)) ?? "https://api.faceit.com";

    public bool InternalApiErrors => Setting(nameof(InternalApiErrors), "true") == "true";

    public bool LogWebHooks => Setting(nameof(LogWebHooks), "true") == "true";

    public Task<string> InternalApiToken()
    {
        var token = Setting(nameof(InternalApiToken));
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("InternalApiToken is not set");
        return Task.FromResult(token);
    }

    public Task<string> InternalUserAgent()
    {
        var agent = Setting(nameof(InternalUserAgent));
        if (string.IsNullOrWhiteSpace(agent)) agent = "FaceitSharp/1.0";
        return Task.FromResult(agent);
    }

    public string? Setting(string key, string? @default = null)
    {
        return _config[_static.Section + ":" + key] ?? @default;
    }
}
