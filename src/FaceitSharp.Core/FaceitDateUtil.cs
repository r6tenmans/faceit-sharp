namespace FaceitSharp.Core;

/// <summary>
/// Utility for working with FaceIT dates
/// </summary>
public static class FaceitDateUtil
{
    /// <summary>
    /// Converts the given milliseconds to a DateTime
    /// </summary>
    /// <param name="milliseconds">The number of milliseconds</param>
    /// <returns>The date time</returns>
    public static DateTime FaceitEpoch(this long milliseconds) => DateTime.UnixEpoch.AddMilliseconds(milliseconds);

    /// <summary>
    /// Converts the given DateTime to milliseconds
    /// </summary>
    /// <param name="dateTime">The date time</param>
    /// <returns>The number of milliseconds</returns>
    public static long FaceitEpoch(this DateTime dateTime) => (long)(dateTime.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;
}
