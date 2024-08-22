namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// Represents a FaceIT hub queue.
/// </summary>
public partial class FaceitQueue
{
    #region Id
    [JsonPropertyName("guid"), Obsolete("Use " + nameof(Id))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonInclude]
    internal string? Guid
    {
        get => null;
        set => Id = value ?? string.Empty;
    }

    /// <summary>
    /// The competition's unique identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    #endregion

    [JsonPropertyName("entityType")]
    public string EntityType { get; set; } = string.Empty;

    [JsonPropertyName("entityId")]
    public string EntityId { get; set; } = string.Empty;

    [JsonPropertyName("queueName")]
    public string QueueName { get; set; } = string.Empty;

    [JsonPropertyName("organizerId")]
    public string OrganizerId { get; set; } = string.Empty;

    [JsonPropertyName("minSkill")]
    public long MinSkill { get; set; }

    [JsonPropertyName("maxSkill")]
    public long MaxSkill { get; set; }

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("verifiedMatching")]
    public bool VerifiedMatching { get; set; }

    [JsonPropertyName("trueLeader")]
    public bool TrueLeader { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("groupSimilar")]
    public bool GroupSimilar { get; set; }

    [JsonPropertyName("captainSelection")]
    public string CaptainSelection { get; set; } = string.Empty;

    [JsonPropertyName("rolesPriority")]
    public string[] RolesPriority { get; set; } = [];

    [JsonPropertyName("open")]
    public bool Open { get; set; }

    [JsonPropertyName("paused")]
    public bool Paused { get; set; }

    [JsonPropertyName("permissions")]
    public Dictionary<string, string[]> Permissions { get; set; } = new();

    [JsonPropertyName("joinType")]
    public JoinTypeData JoinType { get; set; } = new();

    [JsonPropertyName("enableRegionMatching")]
    public bool EnableRegionMatching { get; set; }

    [JsonPropertyName("superMatchEnabled")]
    public bool SuperMatchEnabled { get; set; }

    [JsonPropertyName("queueAlgorithm")]
    public QueueAlgorithmData QueueAlgorithm { get; set; } = new();

    [JsonPropertyName("matchConfiguration")]
    public MatchConfigurationData MatchConfiguration { get; set; } = new();

    [JsonPropertyName("banSetting")]
    public BanSettingData BanSetting { get; set; } = new();

    [JsonPropertyName("noOfPlayers")]
    public long NoOfPlayers { get; set; }

    [JsonPropertyName("checkIn")]
    public CheckInData CheckIn { get; set; } = new();

    [JsonPropertyName("anticheatRequired")]
    public bool AnticheatRequired { get; set; }

    [JsonPropertyName("calculateElo")]
    public bool CalculateElo { get; set; }

    [JsonPropertyName("fbiManagement")]
    public bool FbiManagement { get; set; }

    [JsonPropertyName("adminTool")]
    public bool AdminTool { get; set; }

    [JsonPropertyName("manualResult")]
    public ManualResultData ManualResult { get; set; } = new();

    [JsonPropertyName("afkAction")]
    public string AfkAction { get; set; } = string.Empty;

    [JsonPropertyName("preReqGate")]
    public PreReqGateData PreReqGate { get; set; } = new();

    [JsonPropertyName("sortSelection")]
    public SortSelectionData SortSelection { get; set; } = new();

    [JsonPropertyName("queueCooldownSeconds")]
    public long QueueCooldownSeconds { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTimeOffset LastModified { get; set; }

    [JsonPropertyName("useExactTimeAutoRequeue")]
    public bool UseExactTimeAutoRequeue { get; set; }

    [JsonPropertyName("gverifiedMatchEnabled")]
    public bool GverifiedMatchEnabled { get; set; }

    public partial class BanSettingData
    {
        [JsonPropertyName("noAccept")]
        public NoAccept NoAccept { get; set; } = new();
    }

    public partial class NoAccept
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("banLength")]
        public long BanLength { get; set; }
    }

    public partial class CheckInData
    {
        [JsonPropertyName("time")]
        public long Time { get; set; }
    }

    public partial class JoinTypeData
    {
        [JsonPropertyName("maxParty")]
        public long MaxParty { get; set; }

        [JsonPropertyName("solo")]
        public bool Solo { get; set; }

        [JsonPropertyName("party")]
        public bool Party { get; set; }

        [JsonPropertyName("premade")]
        public bool Premade { get; set; }
    }

    public partial class ManualResultData
    {
        [JsonPropertyName("timeout")]
        public long Timeout { get; set; }

        [JsonPropertyName("onTimeout")]
        public string OnTimeout { get; set; } = string.Empty;
    }

    public partial class MatchConfigurationData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("overview")]
        public Overview Overview { get; set; } = new();

        [JsonPropertyName("tree")]
        public Tree Tree { get; set; } = new();
    }

    public partial class Overview
    {
        [JsonPropertyName("description")]
        public Dictionary<string, string> Description { get; set; } = [];

        [JsonPropertyName("game")]
        public string Game { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public Dictionary<string, string> Label { get; set; } = [];

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

        [JsonPropertyName("client_best_of")]
        public string ClientBestOf { get; set; } = string.Empty;

        [JsonPropertyName("elo_mode")]
        public string EloMode { get; set; } = string.Empty;

        [JsonPropertyName("expire_seconds")]
        public long ExpireSeconds { get; set; }

        [JsonPropertyName("flexible_factions")]
        public bool FlexibleFactions { get; set; }

        [JsonPropertyName("group_id")]
        public Guid GroupId { get; set; }

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
        public string OrganizerId { get; set; } = string.Empty;

        [JsonPropertyName("elo_type")]
        public string EloType { get; set; } = string.Empty;

        [JsonPropertyName("match_configuration_type")]
        public MatchType MatchConfigurationType { get; set; } = new();

        [JsonPropertyName("match_finished_type")]
        public MatchType MatchFinishedType { get; set; } = new();

        [JsonPropertyName("game_type")]
        public string GameType { get; set; } = string.Empty;
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
        public Dictionary<string, string> Label { get; set; } = new();
    }

    public partial class Round
    {
        [JsonPropertyName("label")]
        public Dictionary<string, string> Label { get; set; } = new();

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
        public Dictionary<string, string> Label { get; set; } = new();

        [JsonPropertyName("flags")]
        public MapFlags Flags { get; set; } = new();

        [JsonPropertyName("values")]
        public MapValues Values { get; set; } = new();
    }

    public partial class Display
    {
        [JsonPropertyName("priority")]
        public long Priority { get; set; }
    }

    public partial class MapFlags
    {
        [JsonPropertyName("votable")]
        public bool Votable { get; set; }
    }

    public partial class MapValues
    {
        [JsonPropertyName("value")]
        public Value[] Value { get; set; } = [];
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
    }

    public partial class PreReqGateData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }

    public partial class QueueAlgorithmData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("algorithmId")]
        public string AlgorithmId { get; set; } = string.Empty;

        [JsonPropertyName("geoLabel")]
        public Dictionary<string, string> GeoLabel { get; set; } = [];

        [JsonPropertyName("algorithmParameters")]
        public AlgorithmParameters AlgorithmParameters { get; set; } = new();

        [JsonPropertyName("algorithmInput")]
        public string[] AlgorithmInput { get; set; } = [];

        [JsonPropertyName("roleBasedCaptainPick")]
        public bool RoleBasedCaptainPick { get; set; }
    }

    public partial class AlgorithmParameters
    {
        [JsonPropertyName("band")]
        public Band Band { get; set; } = new();
    }

    public partial class Band
    {
        [JsonPropertyName("value")]
        public long Value { get; set; }
    }

    public partial class SortSelectionData
    {
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
    }
}
