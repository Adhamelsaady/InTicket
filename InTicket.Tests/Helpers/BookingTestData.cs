using InTicket.Application.Feauters.Booking.BookTickets;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Helpers;

public static class BookingTestData
{
    public static readonly Guid HomeTeamId = Guid.Parse("11111111-0000-0000-0000-000000000001");
    public static readonly Guid AwayTeamId = Guid.Parse("22222222-0000-0000-0000-000000000002");
    public static readonly Guid MatchId = Guid.Parse("33333333-0000-0000-0000-000000000003");

    public static DomainMatch OpenMatch() => new DomainMatch
    {
        Id = MatchId,
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        GeneralBookingStart = DateTime.Now.AddDays(-1)
    };

    public static DomainMatch FanPriorityMatch() => new DomainMatch
    {
        Id = MatchId,
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        GeneralBookingStart = DateTime.Now.AddDays(1)
    };

    public static ApplicationUser HomeTeamFan(string id = "user-1") => new ApplicationUser
    {
        Id = id,
        FirstName = "Home",
        LastName = "Fan",
        FavoriteTeamId = HomeTeamId
    };

    public static ApplicationUser AwayTeamFan(string id = "user-1") => new ApplicationUser
    {
        Id = id,
        FirstName = "Away",
        LastName = "Fan",
        FavoriteTeamId = AwayTeamId
    };

    public static MatchTicket AvailableTicket(int price = 100) => new MatchTicket
    {
        TicketId = Guid.NewGuid(),
        Price = price,
        Status = TicketStatus.Open,
        RowVersion = Array.Empty<byte>()
    };

    public static BookMatchTicketsRequest SingleTicketRequest(
        string userId, bool isHomeTeam = true) => new()
    {
        UserId = userId,
        MatchId = MatchId,
        BookingDate = DateTime.UtcNow,
        MatchTicketForBookingDtos = new List<MatchTicketForBookingDto>
        {
            new() { BookingForUserId = userId, isHomeTeam = isHomeTeam, Class = MatchTicketClass.FirstClass_Left }
        }
    };

    public static BookMatchTicketsRequest MultiTicketRequest(
        string userId1, string userId2) => new()
    {
        UserId = userId1,
        MatchId = MatchId,
        BookingDate = DateTime.UtcNow,
        MatchTicketForBookingDtos = new List<MatchTicketForBookingDto>
        {
            new() { BookingForUserId = userId1, isHomeTeam = true, Class = MatchTicketClass.FirstClass_Left  },
            new() { BookingForUserId = userId2, isHomeTeam = true, Class = MatchTicketClass.FirstClass_Right }
        }
    };

    public static BookMatchTicketsRequest DuplicateUserRequest(string userId) => new()
    {
        UserId = userId,
        MatchId = MatchId,
        BookingDate = DateTime.UtcNow,
        MatchTicketForBookingDtos = new List<MatchTicketForBookingDto>
        {
            new() { BookingForUserId = userId, isHomeTeam = true,  Class = MatchTicketClass.FirstClass_Left  },
            new() { BookingForUserId = userId, isHomeTeam = false, Class = MatchTicketClass.SecondClass_Right }
        }
    };
}
