namespace FaceitSharp.Webhooks;

public interface IBaseWebhookPayload { }

public abstract class BaseWebhookPayload : IBaseWebhookPayload
{
    public const string EVENT_HUB_CREATED = "hub_created";
    public const string EVENT_HUB_UPDATED = "hub_updated";
    public const string EVENT_HUB_USER_ADDED = "hub_user_added";
    public const string EVENT_HUB_USER_REMOVED = "hub_user_removed";
    public const string EVENT_HUB_USER_INVITED = "hub_user_invited";
    public const string EVENT_HUB_USER_ROLE_ADDED = "hub_user_role_added";
    public const string EVENT_HUB_USER_ROLE_REMOVED = "hub_user_role_removed";

    public const string EVENT_MATCH_FINISHED = "match_status_finished";
    public const string EVENT_MATCH_CREATED = "match_object_created";
    public const string EVENT_MATCH_CANCELLED = "match_status_cancelled";
    public const string EVENT_MATCH_CONFIGURING = "match_status_configuring";
    public const string EVENT_MATCH_READY = "match_status_ready";
    public const string EVENT_MATCH_DEMO_READY = "match_demo_ready";

    public const string EVENT_TOURNAMENT_STARTED = "championship_started";
    public const string EVENT_TOURNAMENT_FINISHED = "championship_finished";
    public const string EVENT_TOURNAMENT_CANCELLED = "championship_cancelled";
    public const string EVENT_TOURNAMENT_CREATED = "championship_created";

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}