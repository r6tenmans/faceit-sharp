namespace FaceitSharp.Webhooks;

using Api.Internal.Models;

[InterfaceOption(EVENT_HUB_UPDATED)]
public class EventHubUpdated : EventHub
{
    [JsonPropertyName("owner_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OwnerId { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("game")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Game { get; set; }

    [JsonPropertyName("region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Region { get; set; }

    [JsonPropertyName("published")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Published { get; set; }

    [JsonPropertyName("check_game")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CheckGame { get; set; }

    [JsonPropertyName("check_region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CheckRegion { get; set; }

    [JsonPropertyName("slots")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Slots { get; set; }

    [JsonPropertyName("join_permissions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? JoinPermissions { get; set; }

    [JsonPropertyName("min_skill_level")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinSkillLevel { get; set; }

    [JsonPropertyName("max_skill_level")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxSkillLevel { get; set; }

    [JsonPropertyName("owner_roles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? OwnerRoles { get; set; }

    [JsonPropertyName("roles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FaceitRole[]? Roles { get; set; }

    [JsonPropertyName("app_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AppConfigData? AppConfig { get; set; }

    public class AppConfigData
    {
        [JsonPropertyName("apps_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AppsEnabled { get; set; }
    }
}
