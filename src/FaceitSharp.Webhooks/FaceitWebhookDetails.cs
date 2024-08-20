namespace FaceitSharp.Webhooks;

/// <summary>
/// Represents a webhook received from the Faceit API.
/// </summary>
public class FaceitWebhookDetails
{
    /// <summary>
    /// The ID of this specific webhook transaction.
    /// </summary>
    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>
    /// The event type of this webhook.
    /// </summary>
    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the event that triggered the webhook
    /// </summary>
    [JsonPropertyName("event_id")]
    public string EventId { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the third party that triggered the webhook.
    /// </summary>
    [JsonPropertyName("third_party_id")]
    public string ThirdPartyId { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the application that owns the webhook.
    /// </summary>
    [JsonPropertyName("app_id")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// The date/time the webhook was received.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// How many times the webhook has been retried
    /// </summary>
    [JsonPropertyName("retry_count")]
    public int RetryCount { get; set; }

    /// <summary>
    /// The version of the webhook.
    /// </summary>
    [JsonPropertyName("version")]
    public int Version { get; set; }
}