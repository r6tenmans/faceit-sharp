namespace FaceitSharp.Webhooks;

/// <summary>
/// Represents a webhook with a payload received from the Faceit API.
/// </summary>
[JsonConverter(typeof(InterfaceParser<FaceitWebhook>))]
[Interface(typeof(IBaseWebhookPayload), nameof(Event), nameof(Payload), nameof(RawPayload))]
public class FaceitWebhook : FaceitWebhookDetails
{
    /// <summary>
    /// The payload of the webhook
    /// </summary>
    [JsonPropertyName("payload")]
    public IBaseWebhookPayload? Payload { get; set; } = null;

    /// <summary>
    /// The raw JSON of the webhook <see cref="Payload"/>
    /// </summary>
    [JsonIgnore]
    public string? RawPayload { get; set; }
}