namespace FaceitSharp.Webhooks;

public abstract class EventTournament : BaseWebhookPayload
{
    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("organizer_id")]
    public string OrganizerId { get; set; } = string.Empty;
}
