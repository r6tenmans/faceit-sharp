namespace FaceitSharp;

using FaceitSharp.Api.Internal;
using Webhooks;

/// <summary>
/// Allows for easy configuration of FaceITSharp
/// </summary>
public interface IFaceitConfigurationBuilder
{
    /// <summary>
    /// Register a webhook handler with the service collection
    /// </summary>
    /// <typeparam name="T">The type of webhook handler</typeparam>
    /// <param name="transient">Whether or not to register the handler as a transient or singleton service</param>
    /// <returns>The builder for chaining</returns>
    IFaceitConfigurationBuilder WithWebhookHandler<T>(bool transient = true)
        where T : class, IFaceitWebhookEventHandler;

    /// <summary>
    /// Register a webhook handler with the service collection
    /// </summary>
    /// <param name="handler">The webhook handler</param>
    /// <returns>The builder for chaining</returns>
    IFaceitConfigurationBuilder WithWebhookHandler(IFaceitWebhookEventHandler handler);

    /// <summary>
    /// Registers the given configuration object with the service collection
    /// </summary>
    /// <param name="config">The FaceIT service configuration</param>
    /// <returns>The builder for chaining</returns>
    IFaceitConfigurationBuilder WithConfig(IFaceitConfig config);

    /// <summary>
    /// Indicates that the configuration should be loaded from the given section in the (already registered) <see cref="IConfiguration"/>
    /// </summary>
    /// <param name="section">The section of the configuration to use</param>
    /// <returns>The builder for chaining</returns>
    IFaceitConfigurationBuilder WithConfig(string section = "Faceit");

    /// <summary>
    /// Registers the given configuration object with the service collection
    /// </summary>
    /// <param name="config">The FaceIT service configuration</param>
    /// <returns>The builder for chaining</returns>
    IFaceitConfigurationBuilder WithConfig(StaticFaceitConfig config);

    /// <summary>
    /// Registers the a static configuration using the given api token and user-agent
    /// </summary>
    /// <param name="apiToken">The API token to use for requests</param>
    /// <param name="userAgent">The user-agent to use for requests</param>
    /// <returns>The builder for chaining</returns>
    IFaceitConfigurationBuilder WithConfig(string apiToken, string userAgent);
}

internal class FaceitConfigurationBuilder(
    IServiceCollection _services) : IFaceitConfigurationBuilder
{
    private bool _hasWebhookHandler = false;
    private bool _hasConfig = false;

    #region Webhook Handling
    public void AddWebhooks()
    {
        if (!_hasWebhookHandler)
            _services.AddFaceitWebhookHandling();

        _hasWebhookHandler = true;
    }

    public IFaceitConfigurationBuilder WithWebhookHandler(IFaceitWebhookEventHandler handler)
    {
        AddWebhooks();
        _services.AddSingleton(handler);
        return this;
    }

    public IFaceitConfigurationBuilder WithWebhookHandler<T>(bool transient = true)
        where T : class, IFaceitWebhookEventHandler
    {
        AddWebhooks();
        _services.AddFaceitWebhookHandler<T>(transient);
        return this;
    }
    #endregion

    #region Configuration
    public IFaceitConfigurationBuilder WithConfig(IFaceitConfig config)
    {
        _hasConfig = true;
        _services.AddSingleton(config);
        return this;
    }

    public IFaceitConfigurationBuilder WithConfig(string section = "Faceit")
    {
        _hasConfig = true;
        _services.AddTransient<IFaceitConfig, FileFaceitConfig>();
        _services.AddSingleton(new FileFaceitConfigConfig(section));
        return this;
    }

    public IFaceitConfigurationBuilder WithConfig(StaticFaceitConfig config) 
        => WithConfig((IFaceitConfig)config);

    public IFaceitConfigurationBuilder WithConfig(string apiToken, string userAgent)
        => WithConfig(new StaticFaceitConfig(apiToken, userAgent));
    #endregion

    public void Build()
    {
        if (!_hasConfig)
            throw new InvalidOperationException("No Faceit configuration was provided. Did you forget to call `WithConfig`?");

        AddWebhooks();
        _services.AddInternalFaceitApi();
        _services.AddTransient<IFaceitApi, FaceitApi>();
    }
}
