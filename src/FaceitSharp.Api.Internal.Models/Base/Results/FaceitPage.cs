namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The results of a paged API request
/// </summary>
/// <typeparam name="T"></typeparam>
public class FaceitPage<T> : FaceitResult<T[]>
{
    /// <summary>
    /// The offset used in the request
    /// </summary>
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    /// <summary>
    /// The limit used in the request
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}
