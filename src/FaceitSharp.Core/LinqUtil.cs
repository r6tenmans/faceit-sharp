namespace FaceitSharp.Core;

/// <summary>
/// Utility for working with Linq
/// </summary>
public static class LinqUtil
{
    /// <summary>
    /// Shortcut for string.Join
    /// </summary>
    /// <typeparam name="T">The type of item</typeparam>
    /// <param name="input">The items</param>
    /// <param name="joiner">What to join with</param>
    /// <returns>The outputted string</returns>
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
    /// Strips any punctuation from the string
    /// </summary>
    /// <param name="input">The input string</param>
    /// <returns>The string with punctuation stripped out</returns>
    public static string StripPunctuation(this string input)
    {
        return new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
    }

    /// <summary>
    /// Gets all of the words from the given string, stripping punctuation
    /// </summary>
    /// <param name="input">The input string</param>
    /// <param name="lower">Whether or not to run <see cref="string.ToLowerInvariant"/></param>
    /// <param name="splitter">The character to split by</param>
    /// <returns>The words in the string</returns>
    public static string[] Words(this string? input, bool lower = true, char splitter = ' ')
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        if (lower) input = input.ToLowerInvariant();
        return input.StripPunctuation().Split(splitter, StringSplitOptions.RemoveEmptyEntries);
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
