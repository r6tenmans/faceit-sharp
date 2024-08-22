namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents a partial FaceIT competition object  
/// </summary>
public partial class FaceitPartialCompetition
{
    #region Id
    [JsonPropertyName("guid"), Obsolete("Use " + nameof(Id))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? Guid
    {
        get => null;
        set => Id = value ?? string.Empty;
    }

    /// <summary>
    /// The competition's unique identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    #endregion

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("organizerGuid")]
    public string OrganizerId { get; set; } = string.Empty;

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("club")]
    public bool Club { get; set; }

    [JsonPropertyName("relationshipType")]
    public string RelationshipType { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    #region Avatar
    [JsonPropertyName("avatarUrl"), Obsolete("Use " + nameof(Avatar))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? AvatarUrl
    {
        get => null;
        set => Avatar = value;
    }

    /// <summary>
    /// The URL to the competition's avatar
    /// </summary>
    [JsonPropertyName("avatar")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Avatar { get; set; } = null;
    #endregion
}
