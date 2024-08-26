namespace FaceitSharp.Core;

/// <summary>
/// Utility for working with Enums
/// </summary>
public static class EnumUtil
{
    /// <summary>
    /// Gets all of the flags from the given flag
    /// </summary>
    /// <typeparam name="T">The type of Enum flag</typeparam>
    /// <param name="value">The enum flag</param>
    /// <param name="onlyBits">Whether or not to just use raw bit flags</param>
    /// <returns>The flags on the enum</returns>
    public static IEnumerable<T> Flags<T>(this T value, bool onlyBits = false) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        var ops = values.Where(x => value.HasFlag(x));

        if (onlyBits)
            ops = ops.Where(x => ((int)(object)x & ((int)(object)x - 1)) == 0);

        return ops;
    }
}
