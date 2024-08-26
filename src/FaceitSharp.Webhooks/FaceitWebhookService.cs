namespace FaceitSharp.Webhooks;

/// <summary>
/// A service that handles FaceIT webhooks
/// </summary>
public interface IFaceitWebhookService
{
    /// <summary>
    /// Handles a FaceIT webhook payload
    /// </summary>
    /// <param name="json">The JSON string representation of the webhook</param>
    Task Handle(string json);

    /// <summary>
    /// Handles a FaceIT webhook payload
    /// </summary>
    /// <param name="webhook">The webhook to handle</param>
    Task Handle(FaceitWebhook webhook);
}

internal class FaceitWebhookService(
    IServiceProvider _provider,
    ILogger<FaceitWebhookService> _logger,
    FaceitConfig _config) : IFaceitWebhookService
{
    public IEnumerable<IFaceitWebhookEventHandler> GetHandlers()
    {
        return _provider.GetServices<IFaceitWebhookEventHandler>();
    }

    public Task Handle(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new InvalidOperationException("Webhook payload is empty");

        var webhook = JsonSerializer.Deserialize<FaceitWebhook>(json)
            ?? throw new InvalidOperationException("Webhook payload is invalid");
        return Handle(webhook);
    }

    public async Task Handle(FaceitWebhook webhook)
    {
        var handlers = GetHandlers();

        if (_config.Webhooks.LogHooks)
            _logger.LogInformation("[FACEIT WEBHOOK] [event::{event}] >> {id} - Received", 
                webhook.Event, webhook.TransactionId);

        if (!handlers.Any())
        {
            _logger.LogWarning("[FACEIT WEBHOOK] [event::{event}] >> {id} - No webhook handlers registered, skipping",
                webhook.Event, webhook.TransactionId);
            return;
        }

        if (webhook.Payload is null)
        {
            _logger.LogWarning("[FACEIT WEBHOOK] [event::{event}] >> {id} - Payload is empty, cannot handle",
                webhook.Event, webhook.TransactionId);
            return;
        }

        if (webhook.Payload is not BaseWebhookPayload payload)
        {
            _logger.LogWarning("[FACEIT WEBHOOK] [event::{event}] >> {id} - Payload is not {class}, cannot handle", 
                webhook.Event, webhook.TransactionId, nameof(BaseWebhookPayload));
            return;
        }

        foreach(var handler in handlers)
        {
            try
            {
                await Handle(webhook, payload, handler);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[FACEIT WEBHOOK] [event::{event}] >> {id} >> [handler:{class}] - Error occurred while running handler",
                    webhook.Event, webhook.TransactionId, handler.GetType().Name);
            }
        }
    }

    public async Task Handle(FaceitWebhook webhook, BaseWebhookPayload payload, IFaceitWebhookEventHandler handler)
    {
        await handler.All(webhook, payload);

        if (payload is EventTournament tournament) await handler.Tournament(webhook, tournament);
        if (payload is EventMatch match) await handler.Match(webhook, match);
        if (payload is EventHub hub) await handler.Hub(webhook, hub);
        if (payload is EventHubUser user) await handler.HubUser(webhook, user);
        if (payload is EventHubUserRole userRole) await handler.HubUserRole(webhook, userRole);

        switch(payload)
        {
            case EventHubUserAdded ehua: await handler.HubUserAdded(webhook, ehua); break;
            case EventHubUserRemoved ehur: await handler.HubUserRemoved(webhook, ehur); break;
            case EventHubUserInvited ehui: await handler.HubUserInvited(webhook, ehui); break;
            case EventHubUserRoleAdded ehura: await handler.HubUserRoleAdded(webhook, ehura); break;
            case EventHubUserRoleRemoved ehurr: await handler.HubUserRoleRemoved(webhook, ehurr); break;
            case EventHubCreated ehc: await handler.HubCreated(webhook, ehc); break;
            case EventHubUpdated ehu: await handler.HubUpdated(webhook, ehu); break;
            case EventMatchCancelled emc: await handler.MatchCancelled(webhook, emc); break;
            case EventMatchConfiguring emc: await handler.MatchConfiguring(webhook, emc); break;
            case EventMatchCreated emc: await handler.MatchCreated(webhook, emc); break;
            case EventMatchDemo emd: await handler.MatchDemoReady(webhook, emd); break;
            case EventMatchFinished emf: await handler.MatchFinished(webhook, emf); break;
            case EventMatchReady emr: await handler.MatchReady(webhook, emr); break;
            case EventTournamentCancelled etc: await handler.TournamentCancelled(webhook, etc); break;
            case EventTournamentCreated etc: await handler.TournamentCreated(webhook, etc); break;
            case EventTournamentFinished etf: await handler.TournamentFinished(webhook, etf); break;
            case EventTournamentStarted ets: await handler.TournamentStarted(webhook, ets); break;

            default:
                _logger.LogWarning("[FACEIT WEBHOOK] [event::{event}] >> {id} >> [handler::{class}] " +
                    "- Unable to figure out what event handler to use. Is this a new event? Payload: {payload}",
                    webhook.Event, webhook.TransactionId, handler.GetType().Name, webhook.RawPayload);
                break;
        }
    }
}
