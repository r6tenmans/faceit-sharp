namespace FaceitSharp.Api.Internal.Models;

public partial class FaceitHub
{
    [JsonPropertyName("chatSettings")]
    public FaceitChatSettings ChatSettings { get; set; } = new();

    [JsonPropertyName("faceitPointsTotal")]
    public long FaceitPointsTotal { get; set; }

    [JsonPropertyName("faceitPointsEquivalentTotal")]
    public long FaceitPointsEquivalentTotal { get; set; }

    [JsonPropertyName("customPrizesCount")]
    public long CustomPrizesCount { get; set; }

    [JsonPropertyName("guid")]
    public Guid Guid { get; set; }

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("coverImage")]
    public string? CoverImage { get; set; }

    [JsonPropertyName("featuringImage")]
    public string? FeaturingImage { get; set; }

    [JsonPropertyName("backgroundImage")]
    public string? BackgroundImage { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("featured")]
    public bool Featured { get; set; }

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("joinPermissions")]
    public string? JoinPermissions { get; set; }

    [JsonPropertyName("minSkillLevel")]
    public long MinSkillLevel { get; set; }

    [JsonPropertyName("maxSkillLevel")]
    public long MaxSkillLevel { get; set; }

    [JsonPropertyName("currentPlayersJoined")]
    public long CurrentPlayersJoined { get; set; }

    [JsonPropertyName("slots")]
    public long Slots { get; set; }

    [JsonPropertyName("adminToolEnabled")]
    public bool AdminToolEnabled { get; set; }

    [JsonPropertyName("rulesGuid")]
    public Guid RulesGuid { get; set; }

    [JsonPropertyName("organizerGuid")]
    public Guid OrganizerGuid { get; set; }

    [JsonPropertyName("organizerName")]
    public string? OrganizerName { get; set; }

    [JsonPropertyName("roles")]
    public FaceitRole[] Roles { get; set; } = [];

    [JsonPropertyName("checkGame")]
    public bool CheckGame { get; set; }

    [JsonPropertyName("checkRegion")]
    public bool CheckRegion { get; set; }

    [JsonPropertyName("checkGeoLocation")]
    public bool CheckGeoLocation { get; set; }

    [JsonPropertyName("applicationConfiguration")]
    public ApplicationConfigurationData ApplicationConfiguration { get; set; } = new();

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("organizer")]
    public OrganizerData Organizer { get; set; } = new();

    [JsonPropertyName("sensitive")]
    public bool Sensitive { get; set; }

    public partial class ApplicationConfigurationData
    {
        [JsonPropertyName("applicationsEnabled")]
        public bool ApplicationsEnabled { get; set; }

        [JsonPropertyName("applicantInputRequired")]
        public bool ApplicantInputRequired { get; set; }
    }

    public partial class OrganizerData
    {
        [JsonPropertyName("guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
