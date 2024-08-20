namespace FaceitSharp.Core;

public static class FaceitDateUtil
{
    public static DateTime FaceitEpoch(this long milliseconds) => DateTime.UnixEpoch.AddMilliseconds(milliseconds);

    public static long FaceitEpoch(this DateTime dateTime) => (long)(dateTime.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;
}
