namespace FaceitSharp.Api.Internal.Models;

public class FaceitChatSettings
{
    [JsonPropertyName("subscriptionAnnouncements")]
    public string SubscriptionAnnouncements { get; set; } = string.Empty;

    [JsonPropertyName("joinAnnouncements")]
    public bool JoinAnnouncements { get; set; }
}
