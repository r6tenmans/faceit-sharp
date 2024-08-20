namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The result of a collection API request
/// </summary>
/// <typeparam name="T">The type of the payload</typeparam>
public class FaceitCollection<T> : FaceitResult<T[]>
{
    /// <summary>
    /// The size of the collection
    /// </summary>
    [JsonPropertyName("size")]
    public int? Size { get; set; }

    /// <summary>
    /// The page number of the collection
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; set; }

    /// <summary>
    /// The size of the page
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; set; }

    /// <summary>
    /// The total number of pages
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }
}
