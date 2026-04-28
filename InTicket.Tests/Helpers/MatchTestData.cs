using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Matches.Commands.CreateMatch;
using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Features.Matches;

public static class MatchTestData
{
    public static readonly Guid MatchId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    public static readonly Guid HomeTeamId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002");
    public static readonly Guid AwayTeamId = Guid.Parse("cccccccc-0000-0000-0000-000000000003");

    public static CreateMatchRequest ValidCreateMatchRequest() => new()
    {
        Title = "El Clasico",
        Description = "Biggest match of the season",
        EventDate = DateTime.UtcNow.AddDays(30),
        Venue = "Camp Nou",
        Location = "Barcelona, Spain",
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        League = "La Liga",
        Season = "2025/2026",
        Round = 10,
        StadiumName = "Camp Nou",
        TicketsDistribution = new Dictionary<MatchTicketClass, TicketData>
        {
            { MatchTicketClass.FirstClass_Left, new TicketData { Count = 50, Price = 500, IsHomeTeam = true } },
            { MatchTicketClass.VIP, new TicketData { Count = 10, Price = 1000, IsHomeTeam = false } }
        }
    };

    public static CreateMatchRequest ValidCreateMatchRequestNoTickets() => new()
    {
        Title = "Derby Match",
        EventDate = DateTime.UtcNow.AddDays(15),
        Venue = "Santiago Bernabeu",
        Location = "Madrid, Spain",
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        League = "La Liga",
        Season = "2025/2026",
        Round = 11,
        StadiumName = "Santiago Bernabeu",
        TicketsDistribution = new Dictionary<MatchTicketClass, TicketData>()
    };

    public static GetAllMatchesRequest ValidGetAllMatchesRequest(bool isAdmin = false) => new()
    {
        MatchResourceParameters = new MatchResourceParameters(),
        IsRequestedByAdmin = isAdmin
    };

    public static GetMatchByIdRequest ValidGetMatchByIdRequest(Guid? id = null) => new()
    {
        Id = id ?? MatchId,
        IsRequestedByAdmin = false
    };

    public static DomainMatch ActiveMatch(Guid? id = null) => new()
    {
        Id = id ?? MatchId,
        Title = "El Clasico",
        IsActive = true,
        EventDate = DateTime.UtcNow.AddDays(30),
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        League = "La Liga",
        Season = "2025/2026",
        HomeTeam = HomeTeam(),
        AwayTeam = AwayTeam()
    };

    public static DomainMatch InactiveMatch(Guid? id = null) => new()
    {
        Id = id ?? MatchId,
        Title = "Upcoming Match",
        IsActive = false,
        EventDate = DateTime.UtcNow.AddDays(30),
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        League = "La Liga",
        Season = "2025/2026",
        HomeTeam = HomeTeam(),
        AwayTeam = AwayTeam()
    };

    public static Team HomeTeam() => new()
    {
        Id = HomeTeamId,
        Name = "FC Barcelona"
    };

    public static Team AwayTeam() => new()
    {
        Id = AwayTeamId,
        Name = "Real Madrid"
    };

    public static GetMatchResponse MatchResponse(Guid? id = null) => new()
    {
        Id = id ?? MatchId,
        Title = "El Clasico",
        IsActive = true,
        EventDate = DateTime.UtcNow.AddDays(30),
        League = "La Liga",
        Season = "2025/2026",
        HomeTeam = new TeamDto { Id = HomeTeamId, Name = "FC Barcelona" },
        AwayTeam = new TeamDto { Id = AwayTeamId, Name = "Real Madrid" }
    };

    public static PagedResult<DomainMatch> PagedMatchResult(List<DomainMatch> matches) => new()
    {
        Items = matches,
        TotalCount = matches.Count,
        PageNumber = 1,
        PageSize = 10
    };
}