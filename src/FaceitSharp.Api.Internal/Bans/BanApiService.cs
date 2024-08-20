namespace FaceitSharp.Api.Internal;

public interface IBanApiService
{
    IQueueBanApiService Queue { get; }

    IHubBanApiService Hub { get; }
}

internal class BanApiService(
    IQueueBanApiService _queue,
    IHubBanApiService _hub) : IBanApiService
{
    public IQueueBanApiService Queue => _queue;

    public IHubBanApiService Hub => _hub;
}
