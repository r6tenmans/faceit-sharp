namespace FaceitSharp.Core;

public interface IFaceitConfig
{
    string InternalApiUrl { get; }

    bool InternalApiErrors { get; }

    bool LogWebHooks { get; }

    Task<string> InternalApiToken();

    Task<string> InternalUserAgent();

    IFaceitSocketConfig Edge { get; }

    IFaceitSocketConfig Chat { get; }
}

public interface IFaceitSocketConfig
{
    string Uri { get; }

    int ReconnectTimeout { get; }

    int ReconnectTimeoutError { get; }

    int KeepAliveInterval { get; }

    double ResponseTimeout { get; }

    string AppVersion { get; }
}