namespace FaceitSharp.Chat;

/// <summary>
/// Extensions for dependency injection
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Adds the faceit chat client to the services
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for registering against</returns>
    public static IServiceCollection AddFaceitChat(this IServiceCollection services)
    {
        return services
            .AddSingleton<IFaceitChatClient, FaceitChatClient>()
            .AddSingleton<IResourceIdService, ResourceIdService>();
    }
}
