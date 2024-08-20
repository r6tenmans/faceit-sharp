namespace FaceitSharp.Api.Internal.Models;

public partial class FaceitPermissionGroup
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("visible")]
    public bool Visible { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("editable")]
    public bool Editable { get; set; }
}
