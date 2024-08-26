namespace FaceitSharp;

using Api.Internal;
using Chat;
using Webhooks;

/// <summary>
/// The root API interface for FaceITSharp
/// </summary>
public interface IFaceitApi
{
    /// <summary>
    /// Anything related to FaceIT webhooks
    /// </summary>
    IFaceitWebhookService Webhooks { get; }

    /// <summary>
    /// Anything related to the internal FaceIT API
    /// </summary>
    IFaceitInternalApiService Internal { get; }

    /// <summary>
    /// Anything related to the FaceIT chat
    /// </summary>
    IFaceitChatClient Chat { get; }
}

/// <summary>
/// The root API interface for FaceITSharp
/// </summary>
/// <param name="_webhooks"></param>
/// <param name="_internal"></param>
/// <param name="_chat"></param>
public class FaceitApi(
    IFaceitWebhookService _webhooks,
    IFaceitInternalApiService _internal,
    IFaceitChatClient _chat) : IFaceitApi
{
    /// <summary>
    /// Anything related to FaceIT webhooks
    /// </summary>
    public IFaceitWebhookService Webhooks => _webhooks;

    /// <summary>
    /// Anything related to the internal FaceIT API
    /// </summary>
    public IFaceitInternalApiService Internal => _internal;

    /// <summary>
    /// Anything related to the FaceIT chat
    /// </summary>
    public IFaceitChatClient Chat => _chat;

    /// <summary>
    /// Creates an instance of the <see cref="IFaceitApi"/> for use
    /// </summary>
    /// <param name="config">The configuration to use for populating settings</param>
    /// <param name="section">The configuration section to use for the FaceIT settings</param>
    /// <returns>The FaceIT api instance</returns>
    /// <remarks>
    /// Prefer using FaceITSharp in concert with the standard dependency injection schema,
    /// instead of creating instances manually.
    /// </remarks>
    public static IFaceitApi Create(IConfiguration config, string section = "Faceit")
    {
        return Create((x, s) =>
        {
            x.WithConfig(section);
            s.AddSingleton(config);
        });
    }

    /// <summary>
    /// Creates an instance of the <see cref="IFaceitApi"/> for use
    /// </summary>
    /// <param name="config">The FaceIT configuration</param>
    /// <returns>The FaceIT api instance</returns>
    /// <remarks>
    /// Prefer using FaceITSharp in concert with the standard dependency injection schema,
    /// instead of creating instances manually.
    /// </remarks>
    public static IFaceitApi Create(IFaceitConfig config)
    {
        return Create((x, s) =>
        {
            x.WithConfig(config);
        });
    }

    /// <summary>
    /// Creates an instance of the <see cref="IFaceitApi"/> for use
    /// </summary>
    /// <param name="config">The FaceIT configuration</param>
    /// <returns>The FaceIT api instance</returns>
    /// <remarks>
    /// Prefer using FaceITSharp in concert with the standard dependency injection schema,
    /// instead of creating instances manually.
    /// </remarks>
    public static IFaceitApi Create(StaticFaceitConfig config)
    {
        return Create((x, s) =>
        {
            x.WithConfig(config);
        });
    }

    /// <summary>
    /// Creates an instance of the <see cref="IFaceitApi"/> for use
    /// </summary>
    /// <param name="config">How to configure the API and the configuration</param>
    /// <returns>The FaceIT api instance</returns>
    /// <remarks>
    /// Prefer using FaceITSharp in concert with the standard dependency injection schema,
    /// instead of creating instances manually.
    /// </remarks>
    public static IFaceitApi Create(Action<IFaceitConfigurationBuilder, IServiceCollection> config)
    {
        var services = new ServiceCollection();
        services.AddFaceit(x => config(x, services));
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IFaceitApi>();
    }
}
