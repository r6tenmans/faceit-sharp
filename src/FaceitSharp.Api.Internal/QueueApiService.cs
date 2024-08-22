namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to FaceIT hub queues
/// </summary>
public interface IQueueApiService
{
    /// <summary>
    /// Fetches all of the queues for a hub
    /// </summary>
    /// <param name="hubId">The ID of the hub</param>
    /// <returns>All of the queues for the hub</returns>
    Task<FaceitQueue[]> ByHub(string hubId);
}

internal class QueueApiService(IInternalApiService _api) : IQueueApiService
{
    public Task<FaceitQueue[]> ByHub(string hubId)
    {
        return _api.GetMany<FaceitQueue>($"queue/v1/queue/hub/{hubId}");
    }
}
