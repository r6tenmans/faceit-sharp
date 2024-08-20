namespace FaceitSharp.Webhooks;

using Api.Internal.Models;

[InterfaceOption(EVENT_TOURNAMENT_CREATED)]
public class EventTournamentCreated : EventTournament
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("featured")]
    public bool Featured { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("admin_tool_enabled")]
    public bool AdminToolEnabled { get; set; }

    [JsonPropertyName("rulesId")]
    public string RuleId { get; set; } = string.Empty;

    [JsonPropertyName("slots")]
    public int Slots { get; set; }

    [JsonPropertyName("total_rounds")]
    public int TotalRounds { get; set; }

    [JsonPropertyName("total_groups")]
    public int TotalGroups { get; set; }

    [JsonPropertyName("check_in_clear")]
    public DateTime CheckInClear { get; set; }

    [JsonPropertyName("check_in_start")]
    public DateTime CheckInStart { get; set; }

    [JsonPropertyName("subscription_end")]
    public DateTime SubscriptionEnd { get; set; }

    [JsonPropertyName("subscription_start")]
    public DateTime SubscriptionStart { get; set; }

    [JsonPropertyName("assets")]
    public AssetsData Assets { get; set; } = new();

    [JsonPropertyName("roles")]
    public FaceitRole[] Roles { get; set; } = [];

    public class AssetsData
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [JsonPropertyName("background")]
        public string Background { get; set; } = string.Empty;

        [JsonPropertyName("cover")]
        public string Cover { get; set; } = string.Empty;

        [JsonPropertyName("featured")]
        public string Featured { get; set; } = string.Empty;
    }
}
