namespace FaceitSharp.Chat;

/// <summary>
/// A service for caching data from the FaceIT API for use in the chat
/// </summary>
public interface IFaceitChatCacheService
{
    /// <summary>
    /// Clean expired items from the cache
    /// </summary>
    /// <param name="timeoutSec">The number of seconds to wait before cleaning the cache again</param>
    /// <param name="token">The cancellation token for stopping the clean loop</param>
    /// <param name="cacheLifeTimeSec">The number of seconds an individual cache lives for</param>
    void CleanInit(double timeoutSec, double cacheLifeTimeSec, CancellationToken token);

    /// <summary>
    /// Fetch a match from the cache
    /// </summary>
    /// <param name="id">The ID of the match to fetch</param>
    /// <param name="bust">Whether or not to bust the cache</param>
    /// <returns>The match that was fetched</returns>
    Task<FaceitMatch?> Match(string id, bool bust = false);

    /// <summary>
    /// Fetches a hub from the cache
    /// </summary>
    /// <param name="id">The ID of the hub to fetch</param>
    /// <param name="bust">Whether or not to bust the cache</param>
    /// <returns>The hub that was fetched</returns>
    Task<FaceitHub?> Hub(string id, bool bust = false);

    /// <summary>
    /// Fetches a user from the cache
    /// </summary>
    /// <param name="id">The ID of the user to fetch</param>
    /// <param name="bust">Whether or not to bust the cache</param>
    /// <returns>The user that was fetched</returns>
    Task<FaceitUser?> User(string id, bool bust = false);

    /// <summary>
    /// Fetches a tournament from the cache
    /// </summary>
    /// <param name="id">The ID of the tournament to fetch</param>
    /// <param name="bust">Whether or not to bust the cache</param>
    /// <returns>The tournament that was fetched</returns>
    Task<FaceitTournament?> Tournament(string id, bool bust = false);

    /// <summary>
    /// Fetches the current user's profile from the cache
    /// </summary>
    /// <param name="bust">Whether or not to bust the cache</param>
    /// <returns>The current user's profile</returns>
    Task<FaceitUserMe?> Me(bool bust = false);
}

internal class FaceitChatCacheService(
    IFaceitInternalApiService _api) : IFaceitChatCacheService
{
    private CacheItem<FaceitUserMe?>? _currentUser;
    private readonly ConcurrentDictionary<string, CacheItem<FaceitMatch?>> _matches = [];
    private readonly ConcurrentDictionary<string, CacheItem<FaceitHub?>> _hubs = [];
    private readonly ConcurrentDictionary<string, CacheItem<FaceitUser?>> _users = [];
    private readonly ConcurrentDictionary<string, CacheItem<FaceitTournament?>> _tournament = [];
    private CancellationToken? _cleaning;
    private double _timeoutSec = 60 * 30;
    private double _cacheLifeTimeSec = 5 * 60;

    public Task<FaceitMatch?> Match(string id, bool bust = false)
    {
        return Fetch(id, _matches, async () => await _api.Matches.Get(id), bust);
    }

    public Task<FaceitHub?> Hub(string id, bool bust = false)
    {
        return Fetch(id, _hubs, async () => await _api.Hubs.Get(id), bust);
    }

    public Task<FaceitUser?> User(string id, bool bust = false)
    {
        return Fetch(id, _users, async () => await _api.Users.ById(id), bust);
    }

    public Task<FaceitTournament?> Tournament(string id, bool bust = false)
    {
        return Fetch(id, _tournament, async () => await _api.Tournaments.Get(id), bust);
    }

    public async Task<FaceitUserMe?> Me(bool bust = false)
    {
        if (bust && _currentUser is not null)
            _currentUser = null;

        _currentUser ??= new CacheItem<FaceitUserMe?>(_api.Users.Me, _cacheLifeTimeSec);
        return await _currentUser.Get();
    }

    public void CleanInit(double timeoutSec, double cacheLifeTimeSec, CancellationToken token)
    {
        _cacheLifeTimeSec = cacheLifeTimeSec;
        _timeoutSec = timeoutSec;

        if (_cleaning.HasValue && !_cleaning.Value.IsCancellationRequested)
            return;

        _cleaning = token;
        _ = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                CleanExpired(_matches);
                CleanExpired(_hubs);
                CleanExpired(_users);
                CleanExpired(_tournament);
                var timeout = TimeSpan.FromSeconds(_timeoutSec);
                await Task.Delay(timeout, token);
            }

            _cleaning = null;
        }, token);
    }

    public async Task<T?> Fetch<T>(string id, ConcurrentDictionary<string, CacheItem<T?>> cache, Func<Task<T?>> fetch, bool bust)
    {
        if (string.IsNullOrWhiteSpace(id))
            return default;

        if (bust && cache.ContainsKey(id))
            cache.TryRemove(id, out _);

        if (cache.TryGetValue(id, out var item))
            return await item.Get();

        item = new CacheItem<T?>(fetch, _cacheLifeTimeSec);
        cache.TryAdd(id, item);
        return await item.Get();
    }

    public static bool Valid<T>(CacheItem<T> item) => item.Stamp.HasValue && item.Stamp > DateTime.Now;

    public static void CleanExpired<T>(ConcurrentDictionary<string, CacheItem<T>> cache)
    {
        foreach (var (key, item) in cache.ToArray())
        {
            if (!Valid(item))
                cache.TryRemove(key, out _);
        }
    }
}
