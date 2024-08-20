namespace FaceitSharp.Api.Internal.Models;

public class FaceitMatchSetResult
{
    [JsonPropertyName("factions")]
    public Dictionary<string, ScoreResult> Factions { get; set; } = [];

    public class ScoreResult
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }
    }
}
