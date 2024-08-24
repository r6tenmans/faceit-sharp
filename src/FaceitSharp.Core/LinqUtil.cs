namespace FaceitSharp.Core;

public static class LinqUtil
{
    public static string StrJoin<T>(this IEnumerable<T> input, string joiner = " ")
    {
        return string.Join(joiner, input);
    }

    /// <summary>
    /// Forces null or empty strings to be null
    /// </summary>
    /// <param name="input">The inputted string</param>
    /// <returns>The coerced string</returns>
    public static string? ForceNull(this string? input)
    {
        return string.IsNullOrWhiteSpace(input) ? null : input;
    }

    /// <summary>
    /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
    /// </summary>
    /// <typeparam name="T">An enumeration type.</typeparam>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
    /// <returns>An object of the specified enumeration type whose value is represented by value.</returns>
    /// <exception cref="ArgumentNullException">The value parameter is null.</exception>
    /// <exception cref="ArgumentException">The specified type is not an enumeration type, or value is either an empty string or only contains  white space, or value is a name, but not one of the named constants.</exception>
    public static T ParseEnum<T>(this string value, bool ignoreCase = true) where T :
        struct, IComparable, IFormattable, IConvertible
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
        if (!typeof(T).IsEnum)
            throw new ArgumentException("T must be an enumerated type.");
        return Enum.Parse<T>(value, ignoreCase);
    }
}
