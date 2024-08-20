namespace FaceitSharp.Webhooks;

public abstract class EventHub : BaseWebhookPayload
{
    [JsonPropertyName("organizer_id")]
    public string OrganizerId { get; set; } = string.Empty;
}