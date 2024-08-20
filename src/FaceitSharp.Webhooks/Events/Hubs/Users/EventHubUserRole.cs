namespace FaceitSharp.Webhooks;

public abstract class EventHubUserRole : EventHub
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("role_id")]
    public string RoleId { get; set; } = string.Empty;
}