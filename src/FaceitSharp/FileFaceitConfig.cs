namespace FaceitSharp;

using Chat;

internal record class FileFaceitConfigConfig(string Section);

internal class FileFaceitConfig(
    IConfiguration _config,
    FileFaceitConfigConfig _static) : IFaceitConfig
{
    public string InternalApiUrl => Setting(nameof(InternalApiUrl)) ?? "https://api.faceit.com";

    public bool InternalApiErrors => Setting(nameof(InternalApiErrors), "true") == "true";

    public bool LogWebHooks => Setting(nameof(LogWebHooks), "true") == "true";

    public IFaceitSocketConfig Edge => new FileFaceitSocketConfig(
        _config, _static, nameof(Edge), 
        EdgeSocket.DEFAULT_URI, EdgeSocket.DEFAULT_RECONNECT,
        EdgeSocket.DEFAULT_RECONNECT_ERROR, EdgeSocket.DEFAULT_KEEP_ALIVE,
        ChatSocket.DEFAULT_RESPONSE_TIMEOUT, string.Empty);

    public IFaceitSocketConfig Chat => new FileFaceitSocketConfig(
        _config, _static, nameof(Chat), 
        ChatSocket.DEFAULT_URI, ChatSocket.DEFAULT_RECONNECT,
        ChatSocket.DEFAULT_RECONNECT_ERROR, ChatSocket.DEFAULT_KEEP_ALIVE,
        ChatSocket.DEFAULT_RESPONSE_TIMEOUT, ChatSocket.DEFAULT_APP_VERSION);

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

internal class FileFaceitSocketConfig(
    IConfiguration _config,
    FileFaceitConfigConfig _static,
    string _section,
    string _defaultUri,
    int _defaultReconnect,
    int _defaultReconnectError,
    int _defaultKeepAlive,
    double _defaultResponseTimeout,
    string _defaultAppVersion) : IFaceitSocketConfig
{
    public string Uri => Setting(nameof(Uri), _defaultUri) ?? _defaultUri;

    public int ReconnectTimeout => Setting(nameof(ReconnectTimeout), _defaultReconnect);

    public int ReconnectTimeoutError => Setting(nameof(ReconnectTimeoutError), _defaultReconnectError);

    public int KeepAliveInterval => Setting(nameof(KeepAliveInterval), _defaultKeepAlive);

    public double ResponseTimeout => Setting(nameof(ResponseTimeout), _defaultResponseTimeout);

    public string AppVersion => Setting(nameof(AppVersion), _defaultAppVersion) ?? _defaultAppVersion;

    public int Setting(string key, int @default = 0)
    {
        var value = Setting(key, null);
        if (string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out var result))
            return @default;

        return result;
    }

    public double Setting(string key, double @default = 0)
    {
        var value = Setting(key, null);
        if (string.IsNullOrWhiteSpace(value) || !double.TryParse(value, out var result))
            return @default;

        return result;
    }

    public string? Setting(string key, string? @default = null)
    {
        var actKey = $"{_static.Section}:{_section}:{key}";
        return _config[actKey] ?? @default;
    }
}