namespace FaceitSharp.Api.Internal.Models;

[Flags]
public enum FaceitTournamentStatus
{
    started = 1,
    created = 2,
    join = 4,
    checkin = 8,
    seeding = 16,
    adjustment = 32,
    scheduling = 64,
    finished = 128,
}