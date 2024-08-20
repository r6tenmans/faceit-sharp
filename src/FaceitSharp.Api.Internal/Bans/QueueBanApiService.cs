namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to queue bans
/// </summary>
public interface IQueueBanApiService
{
    /// <summary>
    /// Issue a queue ban
    /// </summary>
    /// <param name="ban">The ban to issue</param>
    /// <returns>The resulting queue ban</returns>
    Task<FaceitQueueBan?> Create(FaceitQueueBanRequest ban);

    /// <summary>
    /// Delete a queue ban
    /// </summary>
    /// <param name="id">The ID of the queue ban to delete</param>
    /// <returns></returns>
    Task<FaceitResult?> Delete(string id);

    /// <summary>
    /// All all of the queue bans for the given hub
    /// </summary>
    /// <param name="hubId">The ID of the hub to get the bans from</param>
    /// <param name="active">Whether or not to only get active bans</param>
    /// <param name="offset">The paged offset</param>
    /// <param name="limit">The paged limit</param>
    /// <returns>The queue bans</returns>
    Task<FaceitQueueBan[]> Get(string hubId, bool active = true, int offset = 0, int limit = 20);
}

internal class QueueBanApiService(IInternalApiService _api) : IQueueBanApiService
{
    public Task<FaceitQueueBan?> Create(FaceitQueueBanRequest ban)
    {
        return _api.PostOne<FaceitQueueBan, FaceitQueueBanRequest>("queue/v1/ban", ban);
    }

    public Task<FaceitResult?> Delete(string id)
    {
        return _api.Delete<FaceitResult>($"queue/v1/ban/{id}");
    }

    public async Task<FaceitQueueBan[]> Get(string hubId, bool active = true, int offset = 0, int limit = 20)
    {
        var url = $"queue/v1/ban/hub/{hubId}?active={active}&offset={offset}&limit={limit}";
        var result = await _api.GetOne<FaceitQueueBan[]>(url);
        return result ?? [];
    }
}
