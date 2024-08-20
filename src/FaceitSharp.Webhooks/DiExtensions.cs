namespace FaceitSharp.Webhooks;

public static class DiExtensions
{
    public static IServiceCollection AddFaceitWebhookHandling(this IServiceCollection services)
    {
        return services
            .AddTransient<IFaceitWebhookService, FaceitWebhookService>();
    }

    public static IServiceCollection AddFaceitWebhookHandler<T>(this IServiceCollection services, bool transient = true)
        where T : class, IFaceitWebhookEventHandler
    {
        return transient 
            ? services.AddTransient<IFaceitWebhookEventHandler, T>()
            : services.AddSingleton<IFaceitWebhookEventHandler, T>();
    }
}
