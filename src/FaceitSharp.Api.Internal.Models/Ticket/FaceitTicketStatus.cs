namespace FaceitSharp.Api.Internal.Models;

[Flags]
public enum FaceitTicketStatus
{
    open = 1,
    inProgress = 2,
    closed = 4,

    NotClosed = open | inProgress,
}
