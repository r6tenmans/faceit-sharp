namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to FaceIT matches
/// </summary>
public interface IMatchApiService
{
    /// <summary>
    /// All a single match by ID
    /// </summary>
    /// <param name="id">The ID of the match</param>
    /// <returns>The match data</returns>
    Task<FaceitMatch?> Get(string id);

    /// <summary>
    /// Gets all of the matches from the given hub
    /// </summary>
    /// <param name="hubId">The ID of the hub</param>
    /// <param name="status">The status of the matches to filter by</param>
    /// <param name="page">The page of matches to request</param>
    /// <param name="limit">The total number of matches per page</param>
    /// <returns>The page of matches</returns>
    Task<FaceitMatch[]> FromHub(string hubId, MatchStatus? status = null, int page = 1, int limit = 20);

    /// <summary>
    /// Gets all of the matches from the given hub by status
    /// </summary>
    /// <param name="hubId">The ID of the hub</param>
    /// <param name="status">The status of the matches to filter by</param>
    /// <param name="token">The cancellation token for the request</param>
    /// <returns>All of the matches of the specific status</returns>
    IAsyncEnumerable<FaceitMatch> All(string hubId, MatchStatus status, CancellationToken? token = null);

    /// <summary>
    /// Sets the result of a match
    /// </summary>
    /// <param name="id">The ID of the match to set</param>
    /// <param name="faction1Score">The score of the left side team</param>
    /// <param name="faction2Score">The score of the right side team</param>
    /// <returns>The results of the score set</returns>
    Task<FaceitMatch?> SetResult(string id, int faction1Score, int faction2Score);

    /// <summary>
    /// Cancels the match
    /// </summary>
    /// <param name="id">The ID of the match to cancel</param>
    /// <returns>The results of the cancellation</returns>
    Task<FaceitMatch?> Cancel(string id);
}

internal class MatchApiService(IInternalApiService _api) : IMatchApiService
{
    public Task<FaceitMatch?> Get(string id)
    {
        return _api.GetOne<FaceitMatch>($"match/v2/match/{id}");
    }

    public Task<FaceitMatch[]> FromHub(string hubId, MatchStatus? status = null, int page = 1, int limit = 20)
    {
        var pars = new Dictionary<string, string?>
        {
            ["entityId"] = hubId,
            ["entityType"] = "hub",
            ["offset"] = ((page - 1) * limit).ToString(),
            ["limit"] = limit.ToString()
        };

        if (status.HasValue)
            pars["state"] = status.Value.Flags(true).Select(t => t.ToString()).StrJoin(",");

        var query = pars.Parameterize();
        return _api.GetMany<FaceitMatch>($"match/v2/match{query}");
    }

    public async IAsyncEnumerable<FaceitMatch> All(string hubId, MatchStatus status, CancellationToken? token = null)
    {
        int i = 0, limit = 20;
        while (true)
        {
            if (token?.IsCancellationRequested ?? false) break;

            i++;
            var page = await FromHub(hubId, status, i, limit);
            foreach (var item in page)
                yield return item;

            if (page.Length < limit) break;
        }
    }

    public Task<FaceitMatch?> SetResult(string id, int faction1Score, int faction2Score)
    {
        var data = new FaceitMatchSetResult
        {
            Factions = new()
            {
                ["faction1"] = new() { Score = faction1Score },
                ["faction2"] = new() { Score = faction2Score }
            }
        };

        return _api.PostOne<FaceitMatch, FaceitMatchSetResult>($"match/v2/match-actions/{id}/result/1", data);
    }

    public Task<FaceitMatch?> Cancel(string id)
    {
        var data = new FaceitMatchCancel();

        return _api.PostOne<FaceitMatch, FaceitMatchCancel>($"match/v2/match-actions/{id}/state", data);
    }
}
