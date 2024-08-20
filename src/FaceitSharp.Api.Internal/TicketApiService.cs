namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to tickets
/// </summary>
public interface ITicketApiService
{
    /// <summary>
    /// Gets a single ticket by ID
    /// </summary>
    /// <param name="id">The ID of the ticket</param>
    /// <returns>The faceit ticket</returns>
    Task<FaceitTicket?> Get(string id);

    /// <summary>
    /// Gets a page of tickets by hub or competition
    /// </summary>
    /// <param name="id">The ID of the hub or competition</param>
    /// <param name="type">Whether to get the tickets from a hub or competition</param>
    /// <param name="status">The status of the tickets to fetch</param>
    /// <param name="page">The page of results to fetch</param>
    /// <param name="limit">The size of the page</param>
    /// <returns>The page of tickets</returns>
    Task<FaceitTicket[]> Get(string id, FaceitCompetitionType type, FaceitTicketStatus status, int page, int limit);

    /// <summary>
    /// Gets all of the tickets by hub or competition
    /// </summary>
    /// <param name="id">The ID of the hub or competition</param>
    /// <param name="type">Whether to get the tickets from a hub or competition</param>
    /// <param name="status">The status of the tickets to fetch</param>
    /// <param name="maxRequests">The max number of requests to make</param>
    /// <param name="token">A cancellation token for cancelling the requests at any time</param>
    /// <returns>All of the tickets of a specific status from the given hub or competition</returns>
    IAsyncEnumerable<FaceitTicket> All(string id, FaceitCompetitionType type, FaceitTicketStatus status, int maxRequests = 5, CancellationToken? token = null);
}

internal class TicketApiService(IInternalApiService _api) : ITicketApiService
{
    private const int REQUEST_SIZE = 200;

    public Task<FaceitTicket?> Get(string id) => _api.Get<FaceitTicket>($"tickets/v1/ticket/{id}");

    public async Task<FaceitTicket[]> Get(string hubId, FaceitCompetitionType type, FaceitTicketStatus status, int page, int limit)
    {
        int offset = (page - 1) * limit;
        var statuses = status.Flags(true).Select(t => t.ToString()).StrJoin(",");

        var pars = new Dictionary<string, string?>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString(),
            ["status"] = statuses,
            ["sort"] = "desc",
            ["competitionType"] = type.ToString(),
            ["competitionGuid"] = hubId
        }.Parameterize();

        var request = await _api.Get<FaceitTickets>($"tickets/v1/ticket{pars}");
        return request?.Tickets ?? [];

    }

    public async IAsyncEnumerable<FaceitTicket> All(string hubId, FaceitCompetitionType type, FaceitTicketStatus status, int maxRequests = 5, CancellationToken? token = null)
    {
        int i = 0;
        while (true)
        {
            if (token?.IsCancellationRequested ?? false) break;

            i++;
            var page = await Get(hubId, type, status, i, REQUEST_SIZE);
            foreach (var item in page)
                yield return item;

            if (page.Length < REQUEST_SIZE || i == maxRequests) break;
        }
    }
}
