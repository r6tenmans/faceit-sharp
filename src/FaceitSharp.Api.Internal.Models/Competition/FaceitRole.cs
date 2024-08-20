namespace FaceitSharp.Api.Internal.Models;

public class FaceitRole
{
    #region Id
    [JsonPropertyName("guid"), Obsolete("Use " + nameof(Id))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal string? Guid
    {
        get => null;
        set => Id = value ?? string.Empty;
    }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    #endregion

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("permissionGroups")]
    public FaceitPermissionGroup[] PermissionGroups { get; set; } = [];

    [JsonPropertyName("ranking")]
    public long Ranking { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    #region VisibleOnChat
    [JsonPropertyName("visible_on_chat"), Obsolete("Use " + nameof(VisibleOnChat))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal bool? Visible_On_Chat
    {
        get => null;
        set => VisibleOnChat = value ?? false;
    }

    [JsonPropertyName("visibleOnChat")]
    public bool VisibleOnChat { get; set; }
    #endregion

    [JsonPropertyName("chatVisibilityEditable")]
    public bool ChatVisibilityEditable { get; set; }

    [JsonPropertyName("permissions")]
    public string[] Permissions { get; set; } = [];
}
