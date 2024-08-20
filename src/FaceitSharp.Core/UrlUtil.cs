namespace FaceitSharp.Core;

public static class UrlUtil
{
    public static string Parameterize(this Dictionary<string, string?> pars)
    {
        return pars.Count == 0
            ? string.Empty
            : "?" + string.Join("&", pars
                .Where(t => !string.IsNullOrEmpty(t.Value))
                .Select(x => $"{x.Key}={x.Value}"));
    }
}
