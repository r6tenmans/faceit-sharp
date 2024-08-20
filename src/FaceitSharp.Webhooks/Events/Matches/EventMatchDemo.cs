namespace FaceitSharp.Webhooks;

[InterfaceOption(EVENT_MATCH_DEMO_READY)]
public class EventMatchDemo : EventMatch
{
    [JsonPropertyName("demo_url")]
    public string? DemoUrl { get; set; }
}
