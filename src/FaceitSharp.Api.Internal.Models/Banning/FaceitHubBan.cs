namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents the response to a <see cref="FaceitHubBanRequest"/>
/// </summary>
public class FaceitHubBan : FaceitAuditCreated
{
    /// <summary>
    /// The user that was banned
    /// </summary>
    [JsonPropertyName("user")]
    public FaceitPartialUserWithId User { get; set; } = new();

    /// <summary>
    /// The hub that the user was banned from
    /// </summary>
    [JsonPropertyName("competition")]
    public FaceitPartialCompetition Competition { get; set; } = new();

    /// <summary>
    /// The reason the user was banned
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}
