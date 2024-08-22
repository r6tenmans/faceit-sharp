namespace FaceitSharp.Api.Internal;

using Interop;

public static class DiExtensions
{
    public static IServiceCollection AddInternalFaceitApi(this IServiceCollection services)
    {
        return services
            .AddTransient<IBanApiService, BanApiService>()
            .AddTransient<IQueueBanApiService, QueueBanApiService>()
            .AddTransient<IHubBanApiService, HubBanApiService>()
            .AddTransient<IHubApiService, HubApiService>()
            .AddTransient<IMatchApiService, MatchApiService>()
            .AddTransient<ITicketApiService, TicketApiService>()
            .AddTransient<ITournamentApiService, TournamentApiService>()
            .AddTransient<IUserApiService, UserApiService>()
            .AddTransient<IQueueApiService, QueueApiService>()
            .AddTransient<IFaceitInternalApiService, FaceitInternalApiService>()
            .AddTransient<IInternalApiService, InternalApiService>()
            
            .AddTransient<IFaceitJsonService, FaceitJsonService>()
            .AddTransient<IFaceitCacheService, FaceitCacheService>();
    }
}
