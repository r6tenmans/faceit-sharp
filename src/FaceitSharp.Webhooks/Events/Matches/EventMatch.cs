namespace FaceitSharp.Webhooks;

using Api.Internal.Models;

public abstract class EventMatch : BaseWebhookPayload
{
    [JsonPropertyName("organizer_id")]
    public string OrganizerId { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("entity")]
    public MatchEventEntity Entity { get; set; } = new();

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("started_at")]
    public DateTime? StartedAt { get; set; }

    [JsonPropertyName("finished_at")]
    public DateTime? FinishedAt { get; set; }

    [JsonPropertyName("teams")]
    public MatchEventTeam[] Teams { get; set; } = [];

    public class MatchEventEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class MatchEventTeam
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [JsonPropertyName("leader_id")]
        public string LeaderId { get; set; } = string.Empty;

        [JsonPropertyName("co_leader_id")]
        public string CoLeaderId { get; set; } = string.Empty;

        [JsonPropertyName("substitutions")]
        public int Substitutions { get; set; }

        [JsonPropertyName("roster")]
        public MatchEventUser[] Roster { get; set; } = [];

        [JsonPropertyName("substitutes")]
        public MatchEventUser[] Substitutes { get; set; } = [];
    }

    public class MatchEventUser : FaceitPartialUserWithId
    {
        [JsonPropertyName("game_id")]
        public string GameId { get; set; } = string.Empty;

        [JsonPropertyName("game_name")]
        public string GameName { get; set; } = string.Empty;

        [JsonPropertyName("game_skill_level")]
        public int GameSkillLevel { get; set; }

        [JsonPropertyName("membership")]
        public string Membership { get; set; } = string.Empty;

        [JsonPropertyName("anticheat_required")]
        public bool AntiCheatRequired { get; set; } = false;
    }
}