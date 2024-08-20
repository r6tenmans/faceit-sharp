namespace FaceitSharp.Api.Internal.Models;

public partial class FaceitMatch
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("game")]
    public string Game { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("organizerId")]
    public string OrganizerId { get; set; } = string.Empty;

    [JsonPropertyName("entity")]
    public FaceitEntity Entity { get; set; } = new();

    [JsonPropertyName("entityCustom")]
    public FaceitEntityCustom EntityCustom { get; set; } = new();

    [JsonPropertyName("allowOngoingJoin")]
    public bool AllowOngoingJoin { get; set; }

    [JsonPropertyName("anticheatRequired")]
    public bool AntiCheatRequired { get; set; }

    [JsonPropertyName("anticheatMode")]
    public string AntiCheatMode { get; set; } = string.Empty;

    [JsonPropertyName("calculateElo")]
    public bool CalculateElo { get; set; }

    [JsonPropertyName("skillFeedback")]
    public string SkillFeedback { get; set; } = string.Empty;

    [JsonPropertyName("afkAction")]
    public string AfkAction { get; set; } = string.Empty;

    [JsonPropertyName("fbiManagement")]
    public bool FbiManagement { get; set; }

    [JsonPropertyName("adminTool")]
    public bool AdminTool { get; set; }

    [JsonPropertyName("checkIn")]
    public FaceitCheckIn CheckIn { get; set; } = new();

    [JsonPropertyName("manualResult")]
    public FaceitManualResult ManualResult { get; set; } = new();

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("states")]
    public string[] States { get; set; } = [];

    [JsonPropertyName("teams")]
    public Dictionary<string, FaceitTeam> Teams { get; set; } = [];

    [JsonPropertyName("matchCustom")]
    public FaceitMatchCustom MatchCustom { get; set; } = new();

    [JsonPropertyName("voting")]
    public FaceitVoting Voting { get; set; } = new();

    [JsonPropertyName("maps")]
    public MapElement[] Maps { get; set; } = [];

    [JsonPropertyName("summaryResults")]
    public FaceitSummaryResults SummaryResults { get; set; } = new();

    [JsonPropertyName("results")]
    public FaceitSummaryResults[] Results { get; set; } = [];

    [JsonPropertyName("startedAt")]
    public DateTime? StartedAt { get; set; }

    [JsonPropertyName("finishedAt")]
    public DateTime? FinishedAt { get; set; }

    [JsonPropertyName("timeToConnect")]
    public long TimeToConnect { get; set; }

    [JsonPropertyName("version")]
    public long Version { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("parties")]
    public Party[] Parties { get; set; } = [];

    [JsonPropertyName("rosterWithSubstitutes")]
    public bool RosterWithSubstitutes { get; set; }

    public partial class FaceitCheckIn
    {
        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("totalCheckedIn")]
        public long TotalCheckedIn { get; set; }

        [JsonPropertyName("totalPlayers")]
        public long TotalPlayers { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("checkedIn")]
        public bool CheckedIn { get; set; }
    }

    public partial class FaceitEntity
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public partial class FaceitEntityCustom
    {
        [JsonPropertyName("matcherMatchId")]
        public string MatcherMatchId { get; set; } = string.Empty;

        [JsonPropertyName("parties")]
        public Dictionary<string, string[]> Parties { get; set; } = [];

        [JsonPropertyName("partyQueueDurations")]
        public Dictionary<string, double> PartyQueueDurations { get; set; } = [];

        [JsonPropertyName("queueId")]
        public string QueueId { get; set; } = string.Empty;
    }

    public partial class FaceitManualResult
    {
        [JsonPropertyName("timeout")]
        public long Timeout { get; set; }

        [JsonPropertyName("onTimeout")]
        public string OnTimeout { get; set; } = string.Empty;

        [JsonPropertyName("blocked")]
        public bool Blocked { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("results")]
        public FaceitSummaryResults[] Results { get; set; } = [];

        [JsonPropertyName("agrees")]
        public string[] Agrees { get; set; } = [];
    }

    public partial class FaceitSummaryResults
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("partial")]
        public bool? Partial { get; set; }

        [JsonPropertyName("winner")]
        public string Winner { get; set; } = string.Empty;

        [JsonPropertyName("factions")]
        public Dictionary<string, Faction> Factions { get; set; } = [];

        [JsonPropertyName("ascScore")]
        public bool AscScore { get; set; }
    }

    public partial class Faction
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }
    }

    public partial class MapElement
    {
        [JsonPropertyName("class_name")]
        public string ClassName { get; set; } = string.Empty;

        [JsonPropertyName("game_map_id")]
        public string GameMapId { get; set; } = string.Empty;

        [JsonPropertyName("guid")]
        public string Guid { get; set; } = string.Empty;

        [JsonPropertyName("image_lg")]
        public string ImageLg { get; set; } = string.Empty;

        [JsonPropertyName("image_sm")]
        public string ImageSm { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public partial class FaceitMatchCustom
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
        public Dictionary<string, string> Label { get; set; } = [];
    }

    public partial class Round
    {
        [JsonPropertyName("label")]
        public Dictionary<string, string> Label { get; set; } = [];

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
        public TreeMap Map { get; set; } = new();

        [JsonPropertyName("spectators")]
        public Spectators Spectators { get; set; } = new();
    }

    public partial class TreeMap
    {
        [JsonPropertyName("data_type")]
        public string DataType { get; set; } = string.Empty;

        [JsonPropertyName("display")]
        public Display Display { get; set; } = new();

        [JsonPropertyName("flags")]
        public MapFlags Flags { get; set; } = new();

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public Dictionary<string, string> Label { get; set; } = [];

        [JsonPropertyName("leaf_node")]
        public bool LeafNode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

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
        public MapElement[] Value { get; set; } = [];

        [JsonPropertyName("voting_steps")]
        public string[] VotingSteps { get; set; } = [];
    }

    public partial class Spectators
    {
        [JsonPropertyName("allow_empty")]
        public bool AllowEmpty { get; set; }

        [JsonPropertyName("data_type")]
        public string DataType { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("leaf_node")]
        public bool LeafNode { get; set; }

        [JsonPropertyName("optional")]
        public bool Optional { get; set; }
    }


    public partial class Party
    {
        [JsonPropertyName("partyId")]
        public string PartyId { get; set; } = string.Empty;

        [JsonPropertyName("users")]
        public string[] Users { get; set; } = [];
    }

    public partial class FaceitTeam
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("leader")]
        public string Leader { get; set; } = string.Empty;

        [JsonPropertyName("roster")]
        public Roster[] Roster { get; set; } = [];

        [JsonPropertyName("substituted")]
        public bool Substituted { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;
    }

    public partial class Roster : FaceitPartialUserWithId
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; } = string.Empty;

        [JsonPropertyName("gameName")]
        public string GameName { get; set; } = string.Empty;

        [JsonPropertyName("memberships")]
        public string[] Memberships { get; set; } = [];

        [JsonPropertyName("elo")]
        public long Elo { get; set; }

        [JsonPropertyName("gameSkillLevel")]
        public long GameSkillLevel { get; set; }

        [JsonPropertyName("acReq")]
        public bool AcReq { get; set; }

        [JsonPropertyName("partyId")]
        public string PartyId { get; set; } = string.Empty;
    }

    public partial class FaceitVoting
    {
        [JsonPropertyName("voted_entity_types")]
        public string[] VotedEntityTypes { get; set; } = [];

        [JsonPropertyName("map")]
        public VotingMap Map { get; set; } = new();
    }

    public partial class VotingMap
    {
        [JsonPropertyName("entities")]
        public MapElement[] Entities { get; set; } = [];

        [JsonPropertyName("pick")]
        public string[] Pick { get; set; } = [];
    }
}
