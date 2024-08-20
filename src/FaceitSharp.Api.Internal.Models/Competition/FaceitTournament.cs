namespace FaceitSharp.Api.Internal.Models;

public class FaceitTournament
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("organizerId")]
    public string OrganizerId { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("featured")]
    public bool Featured { get; set; }

    [JsonPropertyName("roles")]
    public FaceitRole[] Roles { get; set; } = [];

    [JsonPropertyName("subscriptionStart")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime SubscriptionStart { get; set; }

    [JsonPropertyName("checkinStart")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime CheckinStart { get; set; }

    [JsonPropertyName("checkinClear")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime CheckinClear { get; set; }

    [JsonPropertyName("subscriptionEnd")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime SubscriptionEnd { get; set; }

    [JsonPropertyName("championshipStart")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime ChampionshipStart { get; set; }

    [JsonPropertyName("slots")]
    public long Slots { get; set; }

    [JsonPropertyName("currentSubscriptions")]
    public long CurrentSubscriptions { get; set; }

    [JsonPropertyName("joinChecks")]
    public JoinChecksData JoinChecks { get; set; } = new();

    [JsonPropertyName("adminToolEnabled")]
    public bool AdminToolEnabled { get; set; }

    [JsonPropertyName("acRequired")]
    public bool AcRequired { get; set; }

    [JsonPropertyName("rulesId")]
    public Guid RulesId { get; set; }

    [JsonPropertyName("full")]
    public bool Full { get; set; }

    [JsonPropertyName("checkinEnabled")]
    public bool CheckinEnabled { get; set; }

    [JsonPropertyName("totalRounds")]
    public long TotalRounds { get; set; }

    [JsonPropertyName("schedule")]
    public Dictionary<string, ScheduleData> Schedule { get; set; } = [];

    [JsonPropertyName("totalGroups")]
    public long TotalGroups { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("subscriptionsLocked")]
    public bool SubscriptionsLocked { get; set; }

    [JsonPropertyName("groupSeedingStrategy")]
    public string GroupSeedingStrategy { get; set; } = string.Empty;

    [JsonPropertyName("roundSeedingStrategy")]
    public string RoundSeedingStrategy { get; set; } = string.Empty;

    [JsonPropertyName("rankingType")]
    public string RankingType { get; set; } = string.Empty;

    [JsonPropertyName("organizer")]
    public OrganizerData Organizer { get; set; } = new();

    [JsonPropertyName("afkAction")]
    public string AfkAction { get; set; } = string.Empty;

    [JsonPropertyName("autoStart")]
    public bool AutoStart { get; set; }

    [JsonPropertyName("registrationMode")]
    public string RegistrationMode { get; set; } = string.Empty;

    [JsonPropertyName("chatEnabled")]
    public bool ChatEnabled { get; set; }

    [JsonPropertyName("calculateElo")]
    public bool CalculateElo { get; set; }

    [JsonPropertyName("resultsEnabled")]
    public bool ResultsEnabled { get; set; }

    [JsonPropertyName("tags")]
    public string[] Tags { get; set; } = [];

    public partial class GroupData
    {
        [JsonPropertyName("capacity")]
        public long Capacity { get; set; }

        [JsonPropertyName("currentRoundNumber")]
        public long CurrentRoundNumber { get; set; }

        [JsonPropertyName("totalRounds")]
        public long TotalRounds { get; set; }
    }

    public partial class JoinChecksData
    {
        [JsonPropertyName("minSkillLevel")]
        public long MinSkillLevel { get; set; }

        [JsonPropertyName("maxSkillLevel")]
        public long MaxSkillLevel { get; set; }

        [JsonPropertyName("whitelistGeoCountriesMinPlayers")]
        public long WhitelistGeoCountriesMinPlayers { get; set; }

        [JsonPropertyName("joinPolicy")]
        public string JoinPolicy { get; set; } = string.Empty;

        [JsonPropertyName("allowedTeamTypes")]
        public string[] AllowedTeamTypes { get; set; } = [];

        [JsonPropertyName("checkRegion")]
        public bool CheckRegion { get; set; }

        [JsonPropertyName("checkGeoLocation")]
        public bool CheckGeoLocation { get; set; }
    }

    public partial class FfaPoints
    {
    }

    public partial class ManualResultConfigurationData
    {
        [JsonPropertyName("timeout")]
        public long Timeout { get; set; }
    }

    public partial class MatchConfigurationData
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("overview")]
        public Overview Overview { get; set; } = new();

        [JsonPropertyName("tree")]
        public Tree Tree { get; set; } = new();
    }

    public partial class Overview
    {
        [JsonPropertyName("description")]
        public DescriptionData Description { get; set; } = new();

        [JsonPropertyName("game")]
        public string Game { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public DescriptionData Label { get; set; } = new();

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("round")]
        public Round Round { get; set; } = new();

        [JsonPropertyName("detections")]
        public Detections Detections { get; set; } = new();

        [JsonPropertyName("spectators")]
        public bool Spectators { get; set; }

        [JsonPropertyName("elo_mode")]
        public string EloMode { get; set; } = string.Empty;

        [JsonPropertyName("expire_seconds")]
        public long ExpireSeconds { get; set; }

        [JsonPropertyName("flexible_factions")]
        public bool FlexibleFactions { get; set; }

        [JsonPropertyName("grouping_stats")]
        public string GroupingStats { get; set; } = string.Empty;

        [JsonPropertyName("max_players")]
        public long MaxPlayers { get; set; }

        [JsonPropertyName("min_players")]
        public long MinPlayers { get; set; }

        [JsonPropertyName("team_size")]
        public long TeamSize { get; set; }

        [JsonPropertyName("time_to_connect")]
        public long TimeToConnect { get; set; }

        [JsonPropertyName("time_out_select_random")]
        public bool TimeOutSelectRandom { get; set; }

        [JsonPropertyName("organizer_id")]
        public Guid OrganizerId { get; set; }

        [JsonPropertyName("elo_type")]
        public string EloType { get; set; } = string.Empty;

        [JsonPropertyName("match_configuration_type")]
        public MatchType MatchConfigurationType { get; set; } = new();

        [JsonPropertyName("match_finished_type")]
        public MatchType MatchFinishedType { get; set; } = new();

        [JsonPropertyName("game_type")]
        public string GameType { get; set; } = string.Empty;
    }

    public partial class DescriptionData
    {
        [JsonPropertyName("en")]
        public string En { get; set; } = string.Empty;
    }

    public partial class Detections
    {
        [JsonPropertyName("afk")]
        public bool Afk { get; set; }

        [JsonPropertyName("leavers")]
        public bool Leavers { get; set; }
    }

    public partial class MatchType
    {
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public DescriptionData Label { get; set; } = new();
    }

    public partial class Round
    {
        [JsonPropertyName("label")]
        public DescriptionData Label { get; set; } = new();

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("to_play")]
        public long ToPlay { get; set; }

        [JsonPropertyName("to_win")]
        public long ToWin { get; set; }
    }

    public partial class Tree
    {
        [JsonPropertyName("map")]
        public Map Map { get; set; } = new();

        [JsonPropertyName("spectators")]
        public Spectators Spectators { get; set; } = new();
    }

    public partial class Map
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("leaf_node")]
        public bool LeafNode { get; set; }

        [JsonPropertyName("data_type")]
        public string DataType { get; set; } = string.Empty;

        [JsonPropertyName("display")]
        public Display Display { get; set; } = new();

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public DescriptionData Label { get; set; } = new();

        [JsonPropertyName("flags")]
        public Flags Flags { get; set; } = new();

        [JsonPropertyName("values")]
        public MapValues Values { get; set; } = new();
    }

    public partial class Display
    {
        [JsonPropertyName("priority")]
        public long Priority { get; set; }
    }

    public partial class Flags
    {
        [JsonPropertyName("votable")]
        public bool Votable { get; set; }
    }

    public partial class MapValues
    {
        [JsonPropertyName("value")]
        public Value[] Value { get; set; } = [];

        [JsonPropertyName("voting_steps")]
        public string[] VotingSteps { get; set; } = [];
    }

    public partial class Value
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("game_map_id")]
        public string GameMapId { get; set; } = string.Empty;

        [JsonPropertyName("class_name")]
        public string ClassName { get; set; } = string.Empty;

        [JsonPropertyName("image_sm")]
        public string ImageSm { get; set; } = string.Empty;

        [JsonPropertyName("image_lg")]
        public string ImageLg { get; set; } = string.Empty;
    }

    public partial class Spectators
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("leaf_node")]
        public bool LeafNode { get; set; }

        [JsonPropertyName("data_type")]
        public string DataType { get; set; } = string.Empty;

        [JsonPropertyName("optional")]
        public bool Optional { get; set; }

        [JsonPropertyName("allow_empty")]
        public bool AllowEmpty { get; set; }

        [JsonPropertyName("flags")]
        public FfaPoints Flags { get; set; } = new();

        [JsonPropertyName("values")]
        public SpectatorsValues Values { get; set; } = new();
    }

    public partial class SpectatorsValues
    {
        [JsonPropertyName("value")]
        public object[] Value { get; set; } = [];
    }

    public partial class OrganizerData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("features")]
        public string[] Features { get; set; } = [];
    }

    public partial class ParticipantsData
    {
        [JsonPropertyName("withReservedSlot")]
        public long WithReservedSlot { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }
    }

    public partial class PrizeConfigurationData
    {
        [JsonPropertyName("prizes")]
        public Prize[] Prizes { get; set; } = [];

        [JsonPropertyName("faceitPoints")]
        public long FaceitPoints { get; set; }
    }

    public partial class Prize
    {
        [JsonPropertyName("start_rank")]
        public long StartRank { get; set; }

        [JsonPropertyName("end_rank")]
        public long EndRank { get; set; }

        [JsonPropertyName("faceit_points")]
        public long FaceitPoints { get; set; }
    }

    public partial class QueueConfigurationData
    {
        [JsonPropertyName("queueJoinStart")]
        public long QueueJoinStart { get; set; }

        [JsonPropertyName("queueJoinEnd")]
        public long QueueJoinEnd { get; set; }

        [JsonPropertyName("queueMatchStart")]
        public long QueueMatchStart { get; set; }

        [JsonPropertyName("queueMatchEnd")]
        public long QueueMatchEnd { get; set; }

        [JsonPropertyName("allowedQueueJoin")]
        public bool AllowedQueueJoin { get; set; }

        [JsonPropertyName("soloJoinOnly")]
        public bool SoloJoinOnly { get; set; }
    }


    public partial class RoundsConfigurationData
    {
        [JsonPropertyName("delimiter")]
        public long Delimiter { get; set; }

        [JsonPropertyName("matches")]
        public long Matches { get; set; }
    }

    public partial class ScheduleData
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("date")]
        public long? Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("startsInSeconds")]
        public long? StartsInSeconds { get; set; }
    }

    public partial class SubstitutionConfigurationData
    {
        [JsonPropertyName("maxSubstitutes")]
        public long MaxSubstitutes { get; set; }

        [JsonPropertyName("maxSubstitutions")]
        public long MaxSubstitutions { get; set; }
    }
}