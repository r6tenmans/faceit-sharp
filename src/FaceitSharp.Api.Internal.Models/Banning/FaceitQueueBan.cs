namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents the response to a <see cref="FaceitQueueBanRequest"/>
/// </summary>
public class FaceitQueueBan : FaceitAuditUpdated
{
    /// <summary>
    /// The ID of the ban
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the organizer who owns the queue
    /// </summary>
    [JsonPropertyName("organizerId")]
    public string OrganizerId { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the entity that the ban was issued in
    /// </summary>
    [JsonPropertyName("entityId")]
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// The type of entity that the ban was issued in
    /// </summary>
    [JsonPropertyName("entityType")]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the user that was banned
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The nickname of the user that was banned
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the queue the ban was issued for
    /// </summary>
    [JsonPropertyName("queueId")]
    public string QueueId { get; set; } = string.Empty;

    /// <summary>
    /// The type of ban
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The reason for the ban
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// The date the ban starts
    /// </summary>
    [JsonPropertyName("banStart")]
    public DateTime BanStart { get; set; }

    /// <summary>
    /// The date the ban ends
    /// </summary>
    [JsonPropertyName("banEnd")]
    public DateTime BanEnd { get; set; }

    /// <summary>
    /// The number of hours the ban should last
    /// </summary>
    [JsonIgnore]
    public double BanDurationHours => (BanEnd - BanStart).TotalHours;

    /// <summary>
    /// Whether or not the ban has expired
    /// </summary>
    [JsonPropertyName("expired")]
    public bool Expired { get; set; }
}
