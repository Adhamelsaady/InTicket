using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using InTicket.Tests.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace InTicket.Tests.Features.Booking;

public class BookMatchTicketsRequestHandlerTests
{
    private readonly BookingHandlerMocks _bookingHandlerMocks;

    public BookMatchTicketsRequestHandlerTests()
    {
        _bookingHandlerMocks = new BookingHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ReturnsFailure()
    {
        // Arrange
        var request = BookingTestData.SingleTicketRequest("user-1");
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync((InTicket.Domain.Match)null!);

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_NeverBeginsTransaction()
    {
        // Arrange
        var request = BookingTestData.SingleTicketRequest("user-1");
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync((InTicket.Domain.Match)null!);

        // Act
        await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        _bookingHandlerMocks.Payments.Verify(x => x.BeginTransactionAsync(), Times.Never);
    }


    [Fact]
    public async Task Handle_WhenSameUserAppearsInRequestTwice_ReturnsFailure()
    {
        const string userId = "user-1";
        var request = BookingTestData.DuplicateUserRequest(userId);
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId));

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenBookingForNonDelegatedUser_ReturnsFailure()
    {
        // Arrange
        const string requesterId = "requester";
        const string otherId = "other-user";
        var request = new InTicket.Application.Feauters.Booking.BookTickets.BookMatchTicketsRequest
        {
            UserId = requesterId,
            MatchId = BookingTestData.MatchId,
            BookingDate = DateTime.UtcNow,
            MatchTicketForBookingDtos = new List<MatchTicketForBookingDto>
            {
                new() { BookingForUserId = otherId, isHomeTeam = true, Class = MatchTicketClass.FirstClass_Left }
            }
        };
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(otherId))
            .ReturnsAsync(BookingTestData.HomeTeamFan(otherId));
        _bookingHandlerMocks.Delegations.Setup(x => x.IsDelegatedAsync(otherId, requesterId))
            .ReturnsAsync(false);

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenBookingForDelegatedUser_ReturnsSuccess()
    {
        // Arrange
        const string requesterId = "requester";
        const string delegateId = "delegate";
        var delegateUser = BookingTestData.HomeTeamFan(delegateId);
        var ticket = BookingTestData.AvailableTicket();
        var request = new InTicket.Application.Feauters.Booking.BookTickets.BookMatchTicketsRequest
        {
            UserId = requesterId,
            MatchId = BookingTestData.MatchId,
            BookingDate = DateTime.UtcNow,
            MatchTicketForBookingDtos = new List<MatchTicketForBookingDto>
            {
                new() { BookingForUserId = delegateId, isHomeTeam = true, Class = MatchTicketClass.FirstClass_Left }
            }
        };
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.Delegations.Setup(x => x.IsDelegatedAsync(delegateId, requesterId))
            .ReturnsAsync(true);
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(delegateId))
            .ReturnsAsync(delegateUser);
        _bookingHandlerMocks.MatchTickets.Setup(x => x.UserHasTicketForMatchAsync(It.IsAny<string>(), request.MatchId))
            .ReturnsAsync(false);
        _bookingHandlerMocks.MatchTickets.Setup(x => x.GetAndLockTicketAsync(It.IsAny<MatchTicketClass>(), request.MatchId, It.IsAny<string>()))
            .ReturnsAsync(ticket);
        _bookingHandlerMocks.Payments.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        _bookingHandlerMocks.Payments.Setup(x => x.AddAsync(It.IsAny<Payment>())).ReturnsAsync(new Payment());

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenFanPriorityActiveAndUserFavoritesDifferentTeam_ReturnsFailure()
    {
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId, isHomeTeam: true);
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.FanPriorityMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(BookingTestData.AwayTeamFan(userId));

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenFanPriorityActiveAndUserFavoritesMatchingTeam_ReturnsSuccess()
    {
        const string userId = "user-1";
        var user = BookingTestData.HomeTeamFan(userId);
        var ticket = BookingTestData.AvailableTicket();
        var request = BookingTestData.SingleTicketRequest(userId, isHomeTeam: true);
        _bookingHandlerMocks.SetupSuccessfulBooking(request, BookingTestData.FanPriorityMatch(), user, ticket);

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenGeneralBookingOpen_AllowsNonFanToBookOppositeTeamSeat()
    {
        const string userId = "user-1";
        var awayFan = BookingTestData.AwayTeamFan(userId); 
        var ticket  = BookingTestData.AvailableTicket();
        var request = BookingTestData.SingleTicketRequest(userId, isHomeTeam: true); 
        _bookingHandlerMocks.SetupSuccessfulBooking(request, BookingTestData.OpenMatch(), awayFan, ticket);
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyHasTicketForMatch_ReturnsFailure()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId));
        _bookingHandlerMocks.MatchTickets.Setup(x => x.UserHasTicketForMatchAsync(userId, request.MatchId))
            .ReturnsAsync(true);

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyHasTicket_NeverBeginsTransaction()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId));
        _bookingHandlerMocks.MatchTickets.Setup(x => x.UserHasTicketForMatchAsync(userId, request.MatchId))
            .ReturnsAsync(true);

        // Act
        await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        _bookingHandlerMocks.Payments.Verify(x => x.BeginTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidSingleTicketRequest_ReturnsSuccess()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.SetupSuccessfulBooking(
            request,
            BookingTestData.OpenMatch(),
            BookingTestData.HomeTeamFan(userId),
            BookingTestData.AvailableTicket(price: 150));

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsTotalPriceAndTicketCount()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.SetupSuccessfulBooking(
            request,
            BookingTestData.OpenMatch(),
            BookingTestData.HomeTeamFan(userId),
            BookingTestData.AvailableTicket(price: 200));

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.TotalTickets);
        Assert.Equal(200, result.TotalPrice);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsNonEmptyPaymentCode()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.SetupSuccessfulBooking(
            request,
            BookingTestData.OpenMatch(),
            BookingTestData.HomeTeamFan(userId),
            BookingTestData.AvailableTicket());

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.PaymentCode);
    }

    [Fact]
    public async Task Handle_WithValidRequest_CommitsTransaction()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.SetupSuccessfulBooking(
            request,
            BookingTestData.OpenMatch(),
            BookingTestData.HomeTeamFan(userId),
            BookingTestData.AvailableTicket());

        // Act
        await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        _bookingHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleTickets_ReturnsSummedTotalPrice()
    {
        const string userId1 = "user-1";
        const string userId2 = "user-2";
        var ticket1 = BookingTestData.AvailableTicket(price: 100);
        var ticket2 = BookingTestData.AvailableTicket(price: 200);
        var request = BookingTestData.MultiTicketRequest(userId1, userId2);

        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId1))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId1));
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId2))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId2));
        _bookingHandlerMocks.Delegations.Setup(x => x.IsDelegatedAsync(userId2, userId1))
            .ReturnsAsync(true);
        _bookingHandlerMocks.MatchTickets.Setup(x => x.UserHasTicketForMatchAsync(It.IsAny<string>(), request.MatchId))
            .ReturnsAsync(false);
        _bookingHandlerMocks.MatchTickets
            .SetupSequence(x => x.GetAndLockTicketAsync(It.IsAny<MatchTicketClass>(), request.MatchId, It.IsAny<string>()))
            .ReturnsAsync(ticket1)
            .ReturnsAsync(ticket2);
        _bookingHandlerMocks.Payments.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        _bookingHandlerMocks.Payments.Setup(x => x.AddAsync(It.IsAny<Payment>())).ReturnsAsync(new Payment());

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.TotalTickets);
        Assert.Equal(300, result.TotalPrice);
    }

    // ── Exception / rollback ─────────────────────────────────────────────────

    [Fact]
    public async Task Handle_WhenTicketLockThrows_ReturnsFailure()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId));
        _bookingHandlerMocks.MatchTickets.Setup(x => x.UserHasTicketForMatchAsync(It.IsAny<string>(), request.MatchId))
            .ReturnsAsync(false);
        _bookingHandlerMocks.MatchTickets
            .Setup(x => x.GetAndLockTicketAsync(It.IsAny<MatchTicketClass>(), request.MatchId, It.IsAny<string>()))
            .ThrowsAsync(new Exception("Concurrency conflict"));

        // Act
        var result = await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WhenTicketLockThrows_RollsBackTransaction()
    {
        // Arrange
        const string userId = "user-1";
        var request = BookingTestData.SingleTicketRequest(userId);
        _bookingHandlerMocks.Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false))
            .ReturnsAsync(BookingTestData.OpenMatch());
        _bookingHandlerMocks.UserManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(BookingTestData.HomeTeamFan(userId));
        _bookingHandlerMocks.MatchTickets.Setup(x => x.UserHasTicketForMatchAsync(It.IsAny<string>(), request.MatchId))
            .ReturnsAsync(false);
        _bookingHandlerMocks.MatchTickets
            .Setup(x => x.GetAndLockTicketAsync(It.IsAny<MatchTicketClass>(), request.MatchId, It.IsAny<string>()))
            .ThrowsAsync(new Exception("Concurrency conflict"));

        // Act
        await _bookingHandlerMocks.Handler.Handle(request, CancellationToken.None);

        // Assert
        _bookingHandlerMocks.Transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _bookingHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
