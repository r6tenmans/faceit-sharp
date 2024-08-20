namespace FaceitSharp.Api.Internal;

/// <summary>
/// Requests related to the internal Faceit API
/// </summary>
public interface IFaceitInternalApiService
{
    /// <summary>
    /// Requests related to bans
    /// </summary>
    IBanApiService Bans { get; }

    /// <summary>
    /// Requests related to FaceIT hubs
    /// </summary>
    IHubApiService Hubs { get; }

    /// <summary>
    /// Requests related to FaceIT matches
    /// </summary>
    IMatchApiService Matches { get; }

    /// <summary>
    /// Requests related to FaceIT tickets
    /// </summary>
    ITicketApiService Tickets { get; }

    /// <summary>
    /// Requests related to FaceIT tournaments
    /// </summary>
    ITournamentApiService Tournaments { get; }

    /// <summary>
    /// Requests related to FaceIT users
    /// </summary>
    IUserApiService Users { get; }
}

internal class FaceitInternalApiService(
    IBanApiService _bans,
    IHubApiService _hubs,
    IMatchApiService _matches,
    ITicketApiService _tickets,
    ITournamentApiService _tournaments,
    IUserApiService _users) : IFaceitInternalApiService
{
    public IBanApiService Bans => _bans;

    public IHubApiService Hubs => _hubs;

    public IMatchApiService Matches => _matches;

    public ITicketApiService Tickets => _tickets;

    public ITournamentApiService Tournaments => _tournaments;

    public IUserApiService Users => _users;
}
