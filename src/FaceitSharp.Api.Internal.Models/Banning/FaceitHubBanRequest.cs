namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The request data for creating a permanent hub ban for FaceIT
/// </summary>
public class FaceitHubBanRequest
{
    /// <summary>
    /// The ID of the hub
    /// </summary>
    [JsonPropertyName("hubId")]
    public required string HubId { get; set; }

    /// <summary>
    /// The reason to ban the user
    /// </summary>
    [JsonPropertyName("reason")]
    public required string Reason { get; set; }

    /// <summary>
    /// The ID of the user
    /// </summary>
    [JsonPropertyName("userId")]
    public required string UserId { get; set; }
}
