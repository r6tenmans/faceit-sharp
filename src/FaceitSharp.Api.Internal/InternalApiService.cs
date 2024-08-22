namespace FaceitSharp.Api.Internal;

using Interop;

internal interface IInternalApiService : IApiService
{
    Task<T?> GetOne<T>(string url);

    Task<T[]> GetMany<T>(string url);

    Task<TResult?> PutOne<TResult, TData>(string url, TData value);

    Task<TResult?> PostOne<TResult, TData>(string url, TData value);

    Task<TResult?> DeleteOne<TResult>(string url);

    Task<bool> DeleteOne(string url);
}

internal class InternalApiService(
    IHttpClientFactory httpFactory,
    IFaceitJsonService json,
    IFaceitCacheService cache,
    IFaceitConfig _config,
    ILogger<InternalApiService> _logger) 
        : ApiService(httpFactory, json, cache, _logger), IInternalApiService
{
    public string MarryURLs(string url)
    {
        if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)) return url;
        return $"{_config.InternalApiUrl.TrimEnd('/')}/{url.TrimStart('/')}";
    }

    public async Task<(string Token, string UserAgent)> GetConfig()
    {
        var token = _config.InternalApiToken();
        var userAgent = _config.InternalUserAgent();
        await Task.WhenAll(token, userAgent);
        return (token.Result, userAgent.Result);
    }

    public override IHttpBuilder Create(string url, IJsonService json, string method = "GET")
    {
        var task = GetConfig();
        task.Wait();
        var (faceitToken, faceitUserAgent) = task.Result;
        var uri = MarryURLs(url);
        return base.Create(uri, json, method).With(c =>
        {
            c.Headers.Add("Authorization", $"Bearer {faceitToken}");
            c.Headers.Add("User-Agent", faceitUserAgent);
        });
    }

    public async Task<T?> GetOne<T>(string url)
    {
        try
        {
            var result = await Get<FaceitResult<T>>(url);
            if (result == null || result.Payload == null) return default;

            return result.Payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running faceit get request: {url}", url);
            if (_config.InternalApiErrors)
                throw;
            return default;
        }
    }

    public async Task<T[]> GetMany<T>(string url)
    {
        try
        {
            var result = await Get<FaceitCollection<T>>(url);
            if (result == null || result.Payload == null) return [];

            return result.Payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running faceit get request: {url}", url);
            if (_config.InternalApiErrors)
                throw;
            return [];
        }
    }

    public async Task<TResult?> PutOne<TResult, TData>(string url, TData value)
    {
        try
        {
            var result = await Put<FaceitResult<TResult>, TData>(url, value);
            if (result == null || result.Payload == null) return default;

            return result.Payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running faceit put request: {url}", url);
            if (_config.InternalApiErrors)
                throw;
            return default;
        }
    }

    public async Task<TResult?> PostOne<TResult, TData>(string url, TData value)
    {
        try
        {
            var result = await Post<FaceitResult<TResult>, TData>(url, value);
            if (result == null || result.Payload == null) return default;

            return result.Payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running faceit post request: {url}", url);
            if (_config.InternalApiErrors)
                throw;
            return default;
        }
    }

    public async Task<TResult?> DeleteOne<TResult>(string url)
    {
        try
        {
            var result = await Delete<FaceitResult<TResult>>(url);
            if (result == null || result.Payload == null) return default;

            return result.Payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running faceit delete request: {url}", url);
            if (_config.InternalApiErrors)
                throw;
            return default;
        }
    }

    public async Task<bool> DeleteOne(string url)
    {
        try
        {
            var result = await Delete<FaceitResult>(url);
            return !(result == null || result.Code != "OPERATION-OK");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running faceit delete request: {url}", url);
            if (_config.InternalApiErrors)
                throw;
            return default;
        }
    }
}
