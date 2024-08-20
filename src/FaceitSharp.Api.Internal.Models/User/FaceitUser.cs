namespace FaceitSharp.Api.Internal.Models;

public class FaceitUser : FaceitPartialUserWithId
{
    [JsonPropertyName("activated_at")]
    public DateTimeOffset ActivatedAt { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("flag")]
    public string Flag { get; set; } = string.Empty;

    [JsonPropertyName("friends")]
    public string[] Friends { get; set; } = [];

    [JsonPropertyName("games")]
    public Dictionary<string, Game> Games { get; set; } = [];

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("memberships")]
    public string[] Memberships { get; set; } = [];

    [JsonPropertyName("phone_verified")]
    public bool PhoneVerified { get; set; }

    [JsonPropertyName("registration_status")]
    public string RegistrationStatus { get; set; } = string.Empty;

    [JsonPropertyName("settings")]
    public SettingsData Settings { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [JsonPropertyName("user_type")]
    public string UserType { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public long Version { get; set; }

    public partial class Game
    {
        [JsonPropertyName("game_id")]
        public string GameId { get; set; } = string.Empty;

        [JsonPropertyName("game_name")]
        public string GameName { get; set; } = string.Empty;

        [JsonPropertyName("faceit_elo")]
        public long FaceitElo { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("region_updated_at")]
        public DateTimeOffset RegionUpdatedAt { get; set; }

        [JsonPropertyName("skill_level")]
        public long SkillLevel { get; set; }

        [JsonPropertyName("skill_level_label")]
        public string SkillLevelLabel { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = [];

        [JsonPropertyName("elo_refreshed_by_user_at")]
        public DateTimeOffset EloRefreshedByUserAt { get; set; }
    }

    public partial class SettingsData
    {
        [JsonPropertyName("language")]
        public string Language { get; set; } = string.Empty;
    }
}
