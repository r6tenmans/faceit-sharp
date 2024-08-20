namespace FaceitSharp.Api.Internal.Models;

public class FaceitTickets
{
    [JsonPropertyName("tickets")]
    public FaceitTicket[] Tickets { get; set; } = [];

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("end")]
    public int End { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}
