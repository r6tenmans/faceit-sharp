namespace FaceitSharp.Api.Internal.Models;

[Flags]
public enum MatchStatus
{
    SUBSTITUTION = 1 << 0,
    CAPTAIN_PICK = 1 << 1,
    VOTING = 1 << 2,
    CONFIGURING = 1 << 3,
    READY = 1 << 4,
    ONGOING = 1 << 5,
    MANUAL_RESULT = 1 << 6,
    PAUSED = 1 << 7,
    ABORTED = 1 << 8,
    FINISHED = 1 << 9,

    Open = SUBSTITUTION | CAPTAIN_PICK | VOTING | CONFIGURING | READY | ONGOING | MANUAL_RESULT | PAUSED | ABORTED,
}
