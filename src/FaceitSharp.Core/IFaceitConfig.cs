namespace FaceitSharp.Core;

public interface IFaceitConfig
{
    string InternalApiUrl { get; }

    bool InternalApiErrors { get; }

    bool LogWebHooks { get; }

    Task<string> InternalApiToken();

    Task<string> InternalUserAgent();
}
