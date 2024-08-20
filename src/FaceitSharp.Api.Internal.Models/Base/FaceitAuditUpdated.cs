namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents an entity that has creation and update audit information
/// </summary>
public partial class FaceitAuditUpdated : FaceitAuditCreated
{
    #region Updated By
    [JsonPropertyName("lastModified"), Obsolete("Use " + nameof(UpdatedAt))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal DateTime? LastModified
    {
        get => null;
        set => UpdatedAt = value ?? DateTime.MinValue;
    }

    /// <summary>
    /// The date the entity was last updated at
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
    #endregion

    /// <summary>
    /// The user that updated the ban
    /// </summary>
    [JsonPropertyName("updatedBy")]
    public FaceitPartialUserWithId UpdatedBy { get; set; } = new();
}
