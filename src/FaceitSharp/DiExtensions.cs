namespace FaceitSharp;

/// <summary>
/// Register all of the faceit services
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Registers all of the FaceIT services
    /// </summary>
    /// <param name="services">The service collection to register against</param>
    /// <param name="bob">The configuration action for changing how FaceTISharp works</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddFaceit(this IServiceCollection services, Action<IFaceitConfigurationBuilder> bob)
    {
        var builder = new FaceitConfigurationBuilder(services);
        bob(builder);
        builder.Register();
        return services;
    }
}
