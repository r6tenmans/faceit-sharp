namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The request data for creating a queue ban for FaceIT 
/// </summary>
public class FaceitQueueBanRequest
{
    #region Ban End Date
    [JsonPropertyName("banEnd")]
    internal long BanEndEpoch { get; set; }

    /// <summary>
    /// The date the ban should end
    /// </summary>
    [JsonIgnore]
    public required DateTime BanEnd
    {
        get => BanEndEpoch.FaceitEpoch();
        set => BanEndEpoch = value.FaceitEpoch();
    }
    #endregion

    #region Ban Start Date
    [JsonPropertyName("banStart")]
    internal long BanStartEpoch { get; set; } = DateTime.UtcNow.FaceitEpoch();

    /// <summary>
    /// The date the ban should start
    /// </summary>
    [JsonIgnore]
    public DateTime BanStart
    {
        get => BanStartEpoch.FaceitEpoch();
        set => BanStartEpoch = value.FaceitEpoch();
    }
    #endregion

    /// <summary>
    /// The ID of the queue to ban the user from
    /// </summary>
    [JsonPropertyName("queueId")]
    public required string QueueId { get; set; }

    /// <summary>
    /// The reason to ban the user
    /// </summary>
    [JsonPropertyName("reason")]
    public required string Reason { get; set; }

    /// <summary>
    /// The ID of the user to ban
    /// </summary>
    [JsonPropertyName("userId")]
    public required string UserId { get; set; }
}
