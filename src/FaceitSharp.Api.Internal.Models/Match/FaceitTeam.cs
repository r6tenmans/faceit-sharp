namespace FaceitSharp.Api.Internal.Models;

public partial class FaceitTeam
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("leader")]
    public string Leader { get; set; } = string.Empty;

    [JsonPropertyName("roster")]
    public RosterItem[] Roster { get; set; } = [];

    [JsonPropertyName("substituted")]
    public bool Substituted { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

    public partial class RosterItem : FaceitPartialUserWithId
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
}
