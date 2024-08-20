namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to faceit hubs and hub memberships
/// </summary>
public interface IHubApiService
{
    /// <summary>
    /// Get a single hub by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The faceit hub</returns>
    Task<FaceitHub?> Get(string id);

    /// <summary>
    /// All a single page of a user's hub memberships
    /// </summary>
    /// <param name="faceitId">The user's Faceit ID</param>
    /// <param name="offset">The offset to use for the page</param>
    /// <param name="limit">The size of the page</param>
    /// <returns>The page of the user's hub memberships</returns>
    Task<FaceitHubMembership[]> Membership(string faceitId, int offset = 0, int limit = 50);

    /// <summary>
    /// Gets all of the user's hub memberships
    /// </summary>
    /// <param name="faceitId">The user's Faceit ID</param>
    /// <returns>All of the user's hub memberships</returns>
    IAsyncEnumerable<FaceitHubMembership> Memberships(string faceitId);

    /// <summary>
    /// Gets all of the tickets for the hub
    /// </summary>
    /// <param name="id">The ID of the hub</param>
    /// <param name="status">The status of the tickets to fetch</param>
    /// <param name="page">The page of results to fetch</param>
    /// <param name="limit">The size of the page</param>
    /// <returns>The page of tickets</returns>
    Task<FaceitTicket[]> Tickets(string id, FaceitTicketStatus status, int page, int limit);

    /// <summary>
    /// Gets all of the tickets by hub
    /// </summary>
    /// <param name="id">The ID of the hub</param>
    /// <param name="status">The status of the tickets to fetch</param>
    /// <param name="maxRequests">The max number of requests to make</param>
    /// <param name="token">A cancellation token for cancelling the requests at any time</param>
    /// <returns>All of the tickets of a specific status from the given hub</returns>
    IAsyncEnumerable<FaceitTicket> Tickets(string id, FaceitTicketStatus status, int maxRequests = 5, CancellationToken? token = null);
}

internal class HubApiService(
    IInternalApiService _api,
    ITicketApiService _tickets) : IHubApiService
{
    public Task<FaceitHub?> Get(string id) => _api.GetOne<FaceitHub>($"hubs/v1/hub/{id}");

    public async Task<FaceitHubMembership[]> Membership(string faceitId, int offset = 0, int limit = 50)
    {
        var url = $"hubs/v1/memberships/{faceitId}?offset={offset}&limit={limit}";
        var request = await _api.GetOne<FaceitPagedCollection<FaceitHubMembership>>(
                $"hubs/v1/user/{faceitId}/membership?limit={limit}&offset={offset}");
        return request?.Items ?? [];
    }

    public async IAsyncEnumerable<FaceitHubMembership> Memberships(string faceitId)
    {
        int i = 0, limit = 50;
        while (true)
        {
            i++;
            int offset = (i - 1) * limit;
            var request = await Membership(faceitId, offset, limit);
            foreach (var item in request)
                yield return item;

            if (request.Length < limit) break;
        }
    }

    public Task<FaceitTicket[]> Tickets(string id, FaceitTicketStatus status, int page, int limit)
    {
        return _tickets.Get(id, FaceitCompetitionType.hub, status, page, limit);
    }

    public IAsyncEnumerable<FaceitTicket> Tickets(string id, FaceitTicketStatus status, int maxRequests = 5, CancellationToken? token = null)
    {
        return _tickets.All(id, FaceitCompetitionType.hub, status, maxRequests, token);
    }
}
