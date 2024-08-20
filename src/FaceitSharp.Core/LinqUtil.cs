namespace FaceitSharp.Core;

public static class LinqUtil
{
    public static string StrJoin<T>(this IEnumerable<T> input, string joiner = " ")
    {
        return string.Join(joiner, input);
    }
}
