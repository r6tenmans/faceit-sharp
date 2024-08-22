namespace FaceitSharp.Api.Internal.Interop;

/// <summary>
/// A service for interfacing with JSON data
/// </summary>
public interface IFaceitJsonService : IJsonService 
{
    /// <summary>
    /// Serializes the given data into an indented JSON string
    /// </summary>
    /// <typeparam name="T">The type of data to serialize</typeparam>
    /// <param name="data">The data to serialize</param>
    /// <returns>The pretty print version of the JSON</returns>
    string? Pretty<T>(T data);
}

internal class FaceitJsonService: SystemTextJsonService, IFaceitJsonService
{
    public static JsonSerializerOptions? DEFAULT_OPTIONS = null;
    public static JsonSerializerOptions DEFAULT_PRETTY_OPTIONS = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public FaceitJsonService() : base(DEFAULT_OPTIONS ??= new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }) { }

    /// <summary>
    /// Serializes the given data into an indented JSON string
    /// </summary>
    /// <typeparam name="T">The type of data to serialize</typeparam>
    /// <param name="data">The data to serialize</param>
    /// <returns>The pretty print version of the JSON</returns>
    public string? Pretty<T>(T data)
    {
        return JsonSerializer.Serialize(data, DEFAULT_PRETTY_OPTIONS);
    }
}
