namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The request data for creating a queue ban for FaceIT 
/// </summary>
public class FaceitQueueBanRequest
{
    #region Ban End Date
    [JsonPropertyName("banEnd"), JsonInclude]
    internal long BanEndEpoch { get; set; }

    /// <summary>
    /// The date the ban should end
    /// </summary>
    [JsonIgnore]
    public DateTime BanEnd
    {
        get => BanEndEpoch.FaceitEpoch();
        set => BanEndEpoch = value.FaceitEpoch();
    }
    #endregion

    #region Ban Start Date
    [JsonPropertyName("banStart"), JsonInclude]
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

    /// <summary>
    /// The number of hours the ban should last
    /// </summary>
    [JsonIgnore]
    public double BanDurationHours
    {
        get => (BanEnd - BanStart).TotalHours;
        set => BanEnd = BanStart.AddHours(value);
    }
    #endregion

    /// <summary>
    /// The ID of the queue to ban the user from
    /// </summary>
    [JsonPropertyName("queueId")]
    public string QueueId { get; set; } = string.Empty;

    /// <summary>
    /// The reason to ban the user
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the user to ban
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;
}
