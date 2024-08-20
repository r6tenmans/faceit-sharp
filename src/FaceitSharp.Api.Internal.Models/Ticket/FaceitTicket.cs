namespace FaceitSharp.Api.Internal.Models;

public abstract class FaceitTicket
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("guid")]
    public string Guid { get; set; } = string.Empty;

    [JsonPropertyName("createdBy")]
    public TicketUser CreatedBy { get; set; } = new();

    [JsonPropertyName("lastModifiedBy")]
    public TicketUser? LastModifiedBy { get; set; }

    [JsonPropertyName("createdAt")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("lastModifiedAt")]
    [JsonConverter(typeof(JsonEpoch))]
    public DateTime LastModifiedAt { get; set; }

    [JsonPropertyName("match")]
    public TicketMatch Match { get; set; } = new();

    [JsonPropertyName("competitionGuid")]
    public string CompetitionGuid { get; set; } = string.Empty;

    [JsonPropertyName("competitionName")]
    public string? CompetitionName { get; set; }

    [JsonPropertyName("assignee")]
    public TicketUser? Assignee { get; set; }

    [JsonPropertyName("assigneeGuid")]
    public string? AssigneeGuid { get; set; }

    [JsonPropertyName("organizerGuid")]
    public string OrganizerGuid { get; set; } = string.Empty;

    [JsonPropertyName("competitionType")]
    public string CompetitionType { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("closed")]
    public bool Closed { get; set; }

    #region History
    [JsonPropertyName("statusHistory"), Obsolete("Use " + nameof(History))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal TicketHistory[]? StatusHistory
    {
        get => null;
        set => History = value ?? [];
    }

    [JsonPropertyName("history")]
    public TicketHistory[] History { get; set; } = [];
    #endregion

    public class TicketHistory
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(JsonEpoch))]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("operatedBy")]
        public TicketUser OperatedBy { get; set; } = new();
    }

    public class TicketUser : FaceitPartialUser
    {
        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("lastModifiedAt")]
        public long? LastModifiedAt { get; set; }

        [JsonPropertyName("accountNonExpired")]
        public bool AccountNonExpired { get; set; }

        [JsonPropertyName("credentialsNonExpired")]
        public bool CredentialsNonExpired { get; set; }

        [JsonPropertyName("accountNonLocked")]
        public bool AccountNonLocked { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    public class TicketTeam
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class TicketMatch
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("guid")]
        public string Guid { get; set; } = string.Empty;

        [JsonPropertyName("createdBy")]
        public TicketUser CreatedBy { get; set; } = new();

        [JsonPropertyName("lastModifiedBy")]
        public TicketUser? LastModifiedBy { get; set; }

        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(JsonEpoch))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("lastModifiedAt")]
        [JsonConverter(typeof(JsonEpoch))]
        public DateTime LastModifiedAt { get; set; }

        [JsonPropertyName("teams")]
        public TicketTeam[] Teams { get; set; } = [];

        [JsonPropertyName("competitionGuid")]
        public string CompetitionGuid { get; set; } = string.Empty;

        [JsonPropertyName("competitionType")]
        public string CompetitionType { get; set; } = string.Empty;

        [JsonPropertyName("joinedPlayers")]
        public string[] JoinedPlayers { get; set; } = [];

        [JsonPropertyName("adminTool")]
        public bool AdminTool { get; set; }

        [JsonPropertyName("entityCustom")]
        public EntityCustom EntityCustom { get; set; } = new();
    }

    public class EntityCustom
    {
        [JsonPropertyName("parties")]
        public Dictionary<string, string[]> Parties { get; set; } = [];

        [JsonPropertyName("matcherMatchId")]
        public string MatcherMatchId { get; set; } = string.Empty;

        [JsonPropertyName("queueId")]
        public string QueueId { get; set; } = string.Empty;

        [JsonPropertyName("partyQueueDurations")]
        public Dictionary<string, double> PartyQueueDurations { get; set; } = [];
    }
}
