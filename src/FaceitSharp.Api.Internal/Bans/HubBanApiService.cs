namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to hub bans
/// </summary>
public interface IHubBanApiService
{
    /// <summary>
    /// Issue a hub ban
    /// </summary>
    /// <param name="ban">The ban to issue</param>
    /// <returns>The resulting hub ban</returns>
    Task<FaceitHubBan?> Create(FaceitHubBanRequest ban);

    /// <summary>
    /// Issue a hub ban
    /// </summary>
    /// <param name="hubId">The ID of the hub</param>
    /// <param name="userId">The ID of the user</param>
    /// <param name="reason">The reason to ban the user</param>
    /// <returns>The resulting hub ban</returns>
    Task<FaceitHubBan?> Create(string hubId, string userId, string reason);

    /// <summary>
    /// Delete a hub ban
    /// </summary>
    /// <param name="hubId">The ID of the hub to delete from</param>
    /// <param name="userId">The user ID of the hub to delete the ban for</param>
    /// <returns>Whether or not the result was successful</returns>
    Task<bool> Delete(string hubId, string userId);

    /// <summary>
    /// All all of the hub bans for the given hub
    /// </summary>
    /// <param name="hubId">The ID of the hub to get the bans from</param>
    /// <param name="offset">The paged offset</param>
    /// <param name="limit">The paged limit</param>
    /// <returns>The hub bans</returns>
    Task<FaceitHubBan[]> Get(string hubId, int offset = 0, int limit = 50);
}

internal class HubBanApiService(IInternalApiService _api) : IHubBanApiService
{
    public Task<FaceitHubBan?> Create(FaceitHubBanRequest ban)
    {
        var url = $"hubs/v1/hub/{ban.HubId}/ban/{ban.UserId}";
        return _api.PostOne<FaceitHubBan, FaceitHubBanRequest>(url, ban);
    }

    public Task<FaceitHubBan?> Create(string hubId, string userId, string reason)
    {
        return Create(new FaceitHubBanRequest
        {
            HubId = hubId,
            UserId = userId,
            Reason = reason
        });
    }

    public async Task<bool> Delete(string hubId, string userId)
    {
        var url = $"hubs/v1/hub/{hubId}/ban/{userId}";
        var result = await _api.Create(url, "DELETE").Result();
        return result?.IsSuccessStatusCode ?? false;
    }

    public async Task<FaceitHubBan[]> Get(string hubId, int offset = 0, int limit = 50)
    {
        var url = $"hubs/v1/hub/{hubId}/ban?offset={offset}&limit={limit}";
        var result = await _api.GetOne<FaceitPagedCollection<FaceitHubBan>>(url);
        return result?.Items ?? [];
    }
}
