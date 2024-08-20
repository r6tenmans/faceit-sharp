namespace FaceitSharp.Webhooks;

using Api.Internal.Models;

[InterfaceOption(EVENT_HUB_CREATED)]
public class EventHubCreated : EventHub
{
    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("check_game")]
    public bool CheckGame { get; set; }

    [JsonPropertyName("check_region")]
    public bool CheckRegion { get; set; }

    [JsonPropertyName("slots")]
    public int Slots { get; set; }

    [JsonPropertyName("join_permissions")]
    public string JoinPermissions { get; set; } = string.Empty;

    [JsonPropertyName("min_skill_level")]
    public int MinSkillLevel { get; set; }

    [JsonPropertyName("max_skill_level")]
    public int MaxSkillLevel { get; set; }

    [JsonPropertyName("owner_roles")]
    public string[] OwnerRoles { get; set; } = [];

    [JsonPropertyName("roles")]
    public FaceitRole[] Roles { get; set; } = [];

    [JsonPropertyName("app_config")]
    public AppConfigData AppConfig { get; set; } = new();

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    public class AppConfigData
    {
        [JsonPropertyName("apps_enabled")]
        public bool AppsEnabled { get; set; }
    }
}
