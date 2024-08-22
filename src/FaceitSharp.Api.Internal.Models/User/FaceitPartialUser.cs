namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents a partial FaceIT user object
/// </summary>
public partial class FaceitPartialUser
{
    #region User Id

    [JsonPropertyName("guid"), Obsolete("Use " + nameof(UserId))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? Guid
    {
        get => null;
        set => UserId = value ?? UserId;
    }

    /// <summary>
    /// The player's unique identifier.
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;
    #endregion

    #region Name
    [JsonPropertyName("nickname"), Obsolete("Use " + nameof(Name))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? Nickname
    {
        get => null;
        set => Name = value ?? Name;
    }

    [JsonPropertyName("username"), Obsolete("Use " + nameof(Name))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? Username
    {
        get => null;
        set => Name = value ?? Name;
    }

    /// <summary>
    /// The player's username / nickname
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    #endregion

    #region Avatar
    [JsonPropertyName("avatarUrl"), Obsolete("Use " + nameof(Avatar))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? AvatarUrl
    {
        get => null;
        set => Avatar = value;
    }

    /// <summary>
    /// The URL to the player's avatar
    /// </summary>
    [JsonPropertyName("avatar")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Avatar { get; set; } = null;
    #endregion
}

/// <summary>
/// Represents a partial FaceIT user object with an ID
/// </summary>
public partial class FaceitPartialUserWithId : FaceitPartialUser
{
    [JsonPropertyName("id"), Obsolete("Use " + nameof(UserId))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? Id
    {
        get => null;
        set => UserId = value ?? UserId;
    }
}