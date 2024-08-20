namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The result of a collection API request
/// </summary>
/// <typeparam name="T">The type of the payload</typeparam>
public class FaceitPagedCollection<T>
{
    /// <summary>
    /// The size of the collection
    /// </summary>
    [JsonPropertyName("items")]
    public T[] Items { get; set; } = [];

    /// <summary>
    /// The offset of the collection
    /// </summary>
    [JsonPropertyName("start")]
    public int Start { get; set; }

    /// <summary>
    /// The limit of the collection
    /// </summary>
    [JsonPropertyName("end")]
    public int End { get; set; }
}
