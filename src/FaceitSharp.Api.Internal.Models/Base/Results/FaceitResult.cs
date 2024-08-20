namespace FaceitSharp.Api.Internal.Models;

/// <summary>
/// The result of an internal API request
/// </summary>
public class FaceitResult
{
    /// <summary>
    /// The time the result was received
    /// </summary>
    [JsonPropertyName("time")]
    public long? Time { get; set; }

    /// <summary>
    /// The environment the result was received from
    /// </summary>
    [JsonPropertyName("env")]
    public string? Environment { get; set; }

    /// <summary>
    /// The version of the result
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// The message of the result
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// The result of the request
    /// </summary>
    [JsonPropertyName("result")]
    public string? Result { get; set; }

    /// <summary>
    /// The code of the result
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }
}

/// <summary>
/// The result of an internal API request with a payload
/// </summary>
/// <typeparam name="T">The type of the payload</typeparam>
public class FaceitResult<T> : FaceitResult
{
    /// <summary>
    /// The payload of the result
    /// </summary>
    [JsonPropertyName("payload")]
    public T? Payload { get; set; }
}