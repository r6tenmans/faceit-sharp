namespace FaceitSharp.Webhooks;

public abstract class EventHubUser : EventHub
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = [];
}