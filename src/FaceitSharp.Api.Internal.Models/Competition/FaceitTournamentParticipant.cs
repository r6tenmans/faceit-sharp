namespace FaceitSharp.Api.Internal.Models;

public class FaceitTournamentParticipant
{
    public required string Id { get; set; }

    public required TChampionship Championship { get; set; }

    public required TTeam Team { get; set; }

    public required string[] Roster { get; set; }

    public required string[] Substitutes { get; set; }

    public string[] Coaches { get; set; } = [];

    public required string Leader { get; set; }

    public required int JoinSkillLevel { get; set; }

    public required string Status { get; set; }

    public required int Group { get; set; }

    public required TRegistration Registration { get; set; }

    public class TRegistration
    {
        public required string Id { get; set; }
    }

    public class TChampionship
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Game { get; set; }

        public required string OrganizerId { get; set; }
    }

    public class TTeam
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Type { get; set; }

        public required string Status { get; set; }

        public required TTeamMember[] Members { get; set; }

        public string? Avatar { get; set; }
    }

    public class TTeamMember : FaceitPartialUserWithId
    {
        public required string Country { get; set; }

        public required int SkillLevel { get; set; }

        public required TTeamGame[] Games { get; set; }
    }

    public class TTeamGame
    {
        public required string Game { get; set; }
    }
}
