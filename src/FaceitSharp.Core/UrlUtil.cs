namespace FaceitSharp.Core;

/// <summary>
/// Utility for working with URLs
/// </summary>
public static class UrlUtil
{
    /// <summary>
    /// Parameterizes the given dictionary
    /// </summary>
    /// <param name="pars">The parameters</param>
    /// <returns>The parameterized url</returns>
    public static string Parameterize(this Dictionary<string, string?> pars)
    {
        return pars.Count == 0
            ? string.Empty
            : "?" + string.Join("&", pars
                .Where(t => !string.IsNullOrEmpty(t.Value))
                .Select(x => $"{x.Key}={x.Value}"));
    }
}
