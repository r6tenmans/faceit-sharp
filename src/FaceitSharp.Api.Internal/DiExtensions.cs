namespace FaceitSharp.Api.Internal;

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
            .AddTransient<IFaceitInternalApiService, FaceitInternalApiService>()
            .AddTransient<IInternalApiService, InternalApiService>();
    }
}
