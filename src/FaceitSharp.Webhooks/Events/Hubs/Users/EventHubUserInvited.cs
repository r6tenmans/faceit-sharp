namespace FaceitSharp.Webhooks;

using Api.Internal.Models;

[InterfaceOption(EVENT_HUB_USER_INVITED)]
public class EventHubUserInvited : EventHub
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("from_user")]
    public FaceitPartialUserWithId From { get; set; } = new();

    [JsonPropertyName("to_user")]
    public FaceitPartialUserWithId To { get; set; } = new();
}
