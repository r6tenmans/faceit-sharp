namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to FaceIT tournaments
/// </summary>
public interface ITournamentApiService
{
    /// <summary>
    /// Get a single tournament by ID
    /// </summary>
    /// <param name="id">The ID of the tournament</param>
    /// <returns>The faceit tournament</returns>
    Task<FaceitTournament?> Get(string id);

    /// <summary>
    /// Gets all of the faceIT tournaments by organizer ID 
    /// </summary>
    /// <param name="id">The organizer ID</param>
    /// <param name="status">The tournament status to filter by</param>
    /// <param name="maxRequests">The max number of requests to make</param>
    /// <param name="token">A cancellation token for cancelling the requests at any time</param>
    /// <returns>All of the tournaments of a specific status from the given organizer</returns>
    IAsyncEnumerable<FaceitTournament> Get(string id, FaceitTournamentStatus status, int maxRequests = 5, CancellationToken? token = null);

    /// <summary>
    /// Gets a single page of a user's tournament participations
    /// </summary>
    /// <param name="id">The ID fo the tournament</param>
    /// <param name="offset">The offset of the page</param>
    /// <param name="limit">The size of the page</param>
    /// <returns>The page of participants</returns>
    Task<FaceitTournamentParticipant[]> Participants(string id, int offset = 0, int limit = 20);

    /// <summary>
    /// Gets all of the teams for the tournament
    /// </summary>
    /// <param name="id">The ID of the tournament</param>
    /// <param name="maxRequests">The max number of requests to make</param>
    /// <param name="token">A cancellation token for cancelling the requests at any time</param>
    /// <returns>All of the participants in the tournament</returns>
    IAsyncEnumerable<FaceitTournamentParticipant> Teams(string id, int maxRequests = 5, CancellationToken? token = null);

    /// <summary>
    /// Gets a single page of the tournaments by organizer
    /// </summary>
    /// <param name="id">The organizer ID</param>
    /// <param name="status">The tournament status to filter by</param>
    /// <param name="asc">Whether to sort in ascending or descending order</param>
    /// <param name="offset">The offset of the page</param>
    /// <param name="limit">The size of the page</param>
    /// <returns>The page of tournaments</returns>
    Task<FaceitTournament[]> ByOrganizer(string id, FaceitTournamentStatus status, bool asc, int offset = 0, int limit = 100);

    /// <summary>
    /// Gets all of the tickets for the tournament
    /// </summary>
    /// <param name="id">The ID of the tournament</param>
    /// <param name="status">The status of the tickets to fetch</param>
    /// <param name="page">The page of results to fetch</param>
    /// <param name="limit">The size of the page</param>
    /// <returns>The page of tickets</returns>
    Task<FaceitTicket[]> Tickets(string id, FaceitTicketStatus status, int page, int limit);

    /// <summary>
    /// Gets all of the tickets by tournament
    /// </summary>
    /// <param name="id">The ID of the tournament</param>
    /// <param name="status">The status of the tickets to fetch</param>
    /// <param name="maxRequests">The max number of requests to make</param>
    /// <param name="token">A cancellation token for cancelling the requests at any time</param>
    /// <returns>All of the tickets of a specific status from the given tournament</returns>
    IAsyncEnumerable<FaceitTicket> Tickets(string id, FaceitTicketStatus status, int maxRequests = 5, CancellationToken? token = null);
}

internal class TournamentApiService(
    IInternalApiService _api,
    ITicketApiService _tickets) : ITournamentApiService
{
    private const int REQUEST_SIZE = 100;
    private const int REQUEST_SIZE_PARTICIPANTS = 20;

    public Task<FaceitTournament?> Get(string id) => _api.GetOne<FaceitTournament>($"championships/v1/championship/{id}");

    public async Task<FaceitTournament[]> ByOrganizer(string id, FaceitTournamentStatus status, bool asc, int offset = 0, int limit = 100)
    {
        var statuses = status.Flags(true).Select(t => t.ToString().Trim()).StrJoin(",");
        var sort = asc ? "asc" : "desc";
        var url = $"championships/v1/championship?organizerId={id}&status={status}&sort={sort}&offset={offset}&limit={limit}";
        var results = await _api.GetOne<FaceitPagedCollection<FaceitTournament>>(url);
        return results?.Items ?? [];
    }

    public async Task<FaceitTournamentParticipant[]> Participants(string id, int offset = 0, int limit = 20)
    {
        var url = $"championships/v1/championship/{id}/subscription?offset={offset}&limit={limit}";
        var results = await _api.GetOne<FaceitPagedCollection<FaceitTournamentParticipant>>(url);
        return results?.Items ?? [];
    }

    public async IAsyncEnumerable<FaceitTournamentParticipant> Teams(string id, int maxRequests = 5, CancellationToken? token = null)
    {
        int i = 0;
        while (true)
        {
            if (token?.IsCancellationRequested ?? false) break;

            i++;
            var page = await Participants(id, (i - 1) * REQUEST_SIZE_PARTICIPANTS, REQUEST_SIZE_PARTICIPANTS);
            foreach (var item in page)
                yield return item;

            if (page.Length < REQUEST_SIZE_PARTICIPANTS || i == maxRequests) break;
        }
    }

    public async IAsyncEnumerable<FaceitTournament> Get(string id, FaceitTournamentStatus status, int maxRequests = 5, CancellationToken? token = null)
    {
        int i = 0;
        while (true)
        {
            if (token?.IsCancellationRequested ?? false) break;

            i++;
            var page = await ByOrganizer(id, status, true, (i - 1) * REQUEST_SIZE, REQUEST_SIZE);
            foreach (var item in page)
                yield return item;

            if (page.Length < REQUEST_SIZE || i == maxRequests) break;
        }
    }
    public Task<FaceitTicket[]> Tickets(string id, FaceitTicketStatus status, int page, int limit)
    {
        return _tickets.Get(id, FaceitCompetitionType.championship, status, page, limit);
    }

    public IAsyncEnumerable<FaceitTicket> Tickets(string id, FaceitTicketStatus status, int maxRequests = 5, CancellationToken? token = null)
    {
        return _tickets.All(id, FaceitCompetitionType.championship, status, maxRequests, token);
    }
}
