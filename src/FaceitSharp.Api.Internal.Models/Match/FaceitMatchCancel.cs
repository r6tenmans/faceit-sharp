namespace FaceitSharp.Api.Internal.Models;

public class FaceitMatchCancel
{
    [JsonPropertyName("state")]
    public string State { get; set; } = "CANCELLED";
}
