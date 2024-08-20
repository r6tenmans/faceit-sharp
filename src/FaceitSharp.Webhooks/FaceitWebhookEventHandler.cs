namespace FaceitSharp.Webhooks;

/// <summary>
/// Indicates a service handles faceit webhooks.
/// </summary>
public interface IFaceitWebhookEventHandler
{
    /// <summary>
    /// Run whenever a faceit webhook is received
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="payload">The webhook payload</param>
    /// <remarks>This is the first method run in the chain</remarks>
    Task All(FaceitWebhookDetails webhook, IBaseWebhookPayload? payload);

    #region Tournaments
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament the event was for</param>
    /// <remarks>This is run before any other tournament specific event, but after <see cref="All(FaceitWebhookDetails, IBaseWebhookPayload?)"/></remarks>
    Task Tournament(FaceitWebhookDetails webhook, EventTournament tournament);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship being created
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was created</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    Task TournamentCreated(FaceitWebhookDetails webhook, EventTournamentCreated tournament);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship starting
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was started</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    Task TournamentStarted(FaceitWebhookDetails webhook, EventTournamentStarted tournament);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship finished
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was finished</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    Task TournamentFinished(FaceitWebhookDetails webhook, EventTournamentFinished tournament);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship cancelled
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was cancelled</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    Task TournamentCancelled(FaceitWebhookDetails webhook, EventTournamentCancelled tournament);
    #endregion

    #region Matches
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match the event was for</param>
    /// <remarks>This is run before any other match specific event, but after <see cref="All(FaceitWebhookDetails, IBaseWebhookPayload?)"/></remarks>
    Task Match(FaceitWebhookDetails webhook, EventMatch match);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being created
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was created</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    Task MatchCreated(FaceitWebhookDetails webhook, EventMatchCreated match);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being ready
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was readied</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    Task MatchReady(FaceitWebhookDetails webhook, EventMatchReady match);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being finished
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was finished</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    Task MatchFinished(FaceitWebhookDetails webhook, EventMatchFinished match);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being cancelled
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was cancelled</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    Task MatchCancelled(FaceitWebhookDetails webhook, EventMatchCancelled match);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being configured
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was configured</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    Task MatchConfiguring(FaceitWebhookDetails webhook, EventMatchConfiguring match);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match demo by ready
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that the demo was uploaded for</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    Task MatchDemoReady(FaceitWebhookDetails webhook, EventMatchDemo match);
    #endregion

    #region Hubs
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="hub">The hub the event was for</param>
    /// <remarks>This is run before any other hub specific event, but after <see cref="All(FaceitWebhookDetails, IBaseWebhookPayload?)"/></remarks>
    Task Hub(FaceitWebhookDetails webhook, EventHub hub);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a hub being created
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="hub">The hub the event was for</param>
    /// <returns>The will run after <see cref="Hub(FaceitWebhookDetails, EventHub)"/></returns>
    Task HubCreated(FaceitWebhookDetails webhook, EventHubCreated hub);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a hub being updated
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="hub">The hub the event was for</param>
    /// <returns>The will run after <see cref="Hub(FaceitWebhookDetails, EventHub)"/></returns>
    Task HubUpdated(FaceitWebhookDetails webhook, EventHubUpdated hub);
    #endregion

    #region Hub Users
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>The will run after <see cref="Hub(FaceitWebhookDetails, EventHub)"/></returns>
    Task HubUser(FaceitWebhookDetails webhook, EventHubUser user);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user being added to a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    Task HubUserAdded(FaceitWebhookDetails webhook, EventHubUserAdded user);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user being removed from a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    Task HubUserRemoved(FaceitWebhookDetails webhook, EventHubUserRemoved user);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user being invited to a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    Task HubUserInvited(FaceitWebhookDetails webhook, EventHubUserInvited user);
    #endregion

    #region Hub User Roles
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user role in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    Task HubUserRole(FaceitWebhookDetails webhook, EventHubUserRole user);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a role being given to a user in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user/role event data</param>
    /// <returns>This will run after <see cref="HubUserRole(FaceitWebhookDetails, EventHubUserRole)"/></returns>
    Task HubUserRoleAdded(FaceitWebhookDetails webhook, EventHubUserRoleAdded user);

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a role being removed from a user in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user/role event data</param>
    /// <returns>This will run after <see cref="HubUserRole(FaceitWebhookDetails, EventHubUserRole)"/></returns>
    Task HubUserRoleRemoved(FaceitWebhookDetails webhook, EventHubUserRoleRemoved user);
    #endregion
}

/// <summary>
/// Provides an easy to use way to handle faceit webhook events
/// </summary>
public abstract class FaceitWebhookEventHandler : IFaceitWebhookEventHandler
{
    /// <summary>
    /// Run whenever a faceit webhook is received
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="payload">The webhook payload</param>
    /// <remarks>This is the first method run in the chain</remarks>
    public virtual Task All(FaceitWebhookDetails webhook, IBaseWebhookPayload? payload) => Task.CompletedTask;

    #region Tournaments
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament the event was for</param>
    /// <remarks>This is run before any other tournament specific event, but after <see cref="All(FaceitWebhookDetails, IBaseWebhookPayload?)"/></remarks>
    public virtual Task Tournament(FaceitWebhookDetails webhook, EventTournament tournament) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship being created
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was created</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    public virtual Task TournamentCreated(FaceitWebhookDetails webhook, EventTournamentCreated tournament) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship starting
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was started</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    public virtual Task TournamentStarted(FaceitWebhookDetails webhook, EventTournamentStarted tournament) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship finished
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was finished</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    public virtual Task TournamentFinished(FaceitWebhookDetails webhook, EventTournamentFinished tournament) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a tournament / championship cancelled
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="tournament">The tournament that was cancelled</param>
    /// <remarks>This will run after <see cref="Tournament(FaceitWebhookDetails, EventTournament)"/></remarks>
    public virtual Task TournamentCancelled(FaceitWebhookDetails webhook, EventTournamentCancelled tournament) => Task.CompletedTask;
    #endregion

    #region Matches
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match the event was for</param>
    /// <remarks>This is run before any other match specific event, but after <see cref="All(FaceitWebhookDetails, IBaseWebhookPayload?)"/></remarks>
    public virtual Task Match(FaceitWebhookDetails webhook, EventMatch match) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being created
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was created</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    public virtual Task MatchCreated(FaceitWebhookDetails webhook, EventMatchCreated match) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being ready
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was readied</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    public virtual Task MatchReady(FaceitWebhookDetails webhook, EventMatchReady match) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being finished
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was finished</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    public virtual Task MatchFinished(FaceitWebhookDetails webhook, EventMatchFinished match) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being cancelled
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was cancelled</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    public virtual Task MatchCancelled(FaceitWebhookDetails webhook, EventMatchCancelled match) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match being configured
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that was configured</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    public virtual Task MatchConfiguring(FaceitWebhookDetails webhook, EventMatchConfiguring match) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a match demo by ready
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="match">The match that the demo was uploaded for</param>
    /// <remarks>This will run after <see cref="Match(FaceitWebhookDetails, EventMatch)"/></remarks>
    public virtual Task MatchDemoReady(FaceitWebhookDetails webhook, EventMatchDemo match) => Task.CompletedTask;
    #endregion

    #region Hubs
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="hub">The hub the event was for</param>
    /// <remarks>This is run before any other hub specific event, but after <see cref="All(FaceitWebhookDetails, IBaseWebhookPayload?)"/></remarks>
    public virtual Task Hub(FaceitWebhookDetails webhook, EventHub hub) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a hub being created
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="hub">The hub the event was for</param>
    /// <returns>The will run after <see cref="Hub(FaceitWebhookDetails, EventHub)"/></returns>
    public virtual Task HubCreated(FaceitWebhookDetails webhook, EventHubCreated hub) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a hub being updated
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="hub">The hub the event was for</param>
    /// <returns>The will run after <see cref="Hub(FaceitWebhookDetails, EventHub)"/></returns>
    public virtual Task HubUpdated(FaceitWebhookDetails webhook, EventHubUpdated hub) => Task.CompletedTask;
    #endregion

    #region Hub Users
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>The will run after <see cref="Hub(FaceitWebhookDetails, EventHub)"/></returns>
    public virtual Task HubUser(FaceitWebhookDetails webhook, EventHubUser user) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user being added to a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    public virtual Task HubUserAdded(FaceitWebhookDetails webhook, EventHubUserAdded user) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user being removed from a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    public virtual Task HubUserRemoved(FaceitWebhookDetails webhook, EventHubUserRemoved user) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user being invited to a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    public virtual Task HubUserInvited(FaceitWebhookDetails webhook, EventHubUserInvited user) => Task.CompletedTask;
    #endregion

    #region Hub User Roles
    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a user role in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user event data</param>
    /// <returns>This will run after <see cref="HubUser(FaceitWebhookDetails, EventHubUser)"/></returns>
    public virtual Task HubUserRole(FaceitWebhookDetails webhook, EventHubUserRole user) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a role being given to a user in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user/role event data</param>
    /// <returns>This will run after <see cref="HubUserRole(FaceitWebhookDetails, EventHubUserRole)"/></returns>
    public virtual Task HubUserRoleAdded(FaceitWebhookDetails webhook, EventHubUserRoleAdded user) => Task.CompletedTask;

    /// <summary>
    /// Run whenever a faceit webhook is received that relates to a role being removed from a user in a hub
    /// </summary>
    /// <param name="webhook">Details about the webhook event</param>
    /// <param name="user">The hub/user/role event data</param>
    /// <returns>This will run after <see cref="HubUserRole(FaceitWebhookDetails, EventHubUserRole)"/></returns>
    public virtual Task HubUserRoleRemoved(FaceitWebhookDetails webhook, EventHubUserRoleRemoved user) => Task.CompletedTask;
    #endregion
}
