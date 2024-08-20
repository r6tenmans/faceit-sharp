namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents an entity that has creation audit information
/// </summary>
public partial class FaceitAuditCreated
{
    #region Created At
    /// <summary>
    /// The date the entity was created at
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    #endregion

    /// <summary>
    /// The user that created the entity
    /// </summary>
    [JsonPropertyName("createdBy")]
    public FaceitPartialUserWithId CreatedBy { get; set; } = new();
}
