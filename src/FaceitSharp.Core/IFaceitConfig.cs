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

    double ReconnectTimeout { get; }

    double ReconnectTimeoutError { get; }

    double KeepAliveInterval { get; }

    double ResponseTimeout { get; }

    string AppVersion { get; }
}