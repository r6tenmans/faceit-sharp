namespace FaceitSharp.Core;

public static class EnumUtil
{
    public static IEnumerable<T> Flags<T>(this T value, bool onlyBits = false) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        var ops = values.Where(x => value.HasFlag(x));

        if (onlyBits)
            ops = ops.Where(x => ((int)(object)x & ((int)(object)x - 1)) == 0);

        return ops;
    }

    public static IEnumerable<T> Flags<T>(this T value, Func<T, bool> predicate) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        return values.Where(x => value.HasFlag(x) && predicate(x));
    }

    public static IEnumerable<T> AllFlags<T>(this T _) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}
