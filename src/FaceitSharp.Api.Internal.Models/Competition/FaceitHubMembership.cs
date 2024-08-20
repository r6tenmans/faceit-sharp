namespace FaceitSharp.Api.Internal.Models;

public partial class FaceitHubMembership
{
    [JsonPropertyName("user")]
    public FaceitPartialUserWithId User { get; set; } = new();

    [JsonPropertyName("competition")]
    public FaceitHub Competition { get; set; } = new();

    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = [];

    [JsonPropertyName("highestRoleRanking")]
    public long HighestRoleRanking { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    public partial class MembershipRole
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("permissionGroups")]
        public FaceitPermissionGroup[] PermissionGroups { get; set; } = [];

        [JsonPropertyName("ranking")]
        public long Ranking { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("visibleOnChat")]
        public bool VisibleOnChat { get; set; }

        [JsonPropertyName("chatVisibilityEditable")]
        public bool ChatVisibilityEditable { get; set; }
    }
}
