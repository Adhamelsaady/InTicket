using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Booking.BookTickets;
using InTicket.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace InTicket.Tests.Helpers;

public class BookingHandlerMocks
{
    public Mock<IDelegationsRepository>    Delegations  { get; } = new();
    public Mock<IMatchRepository>          Matches      { get; } = new();
    public Mock<UserManager<ApplicationUser>> UserManager { get; }
    public Mock<IMatchTicketRepository>    MatchTickets { get; } = new();
    public Mock<IBaseRepository<Payment>>  Payments     { get; } = new();
    public Mock<IDbContextTransaction>     Transaction  { get; } = new();
    public BookMatchTicketsRequestHandler  Handler      { get; }

    public BookingHandlerMocks()
    {
        UserManager = MockHelpers.MockUserManager();
        Transaction.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Transaction.Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Transaction.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        Payments.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Transaction.Object);

        Handler = new BookMatchTicketsRequestHandler(
            Delegations.Object,
            Matches.Object,
            UserManager.Object,
            MatchTickets.Object,
            Payments.Object);
    }
    public void SetupSuccessfulBooking(
        BookMatchTicketsRequest request,
        InTicket.Domain.Match   match,
        ApplicationUser         user,
        MatchTicket             ticket)
    {
        Matches.Setup(x => x.GetMatchByIdAsync(request.MatchId, false)).ReturnsAsync(match);
        UserManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);
        MatchTickets
            .Setup(x => x.UserHasTicketForMatchAsync(It.IsAny<string>(), request.MatchId))
            .ReturnsAsync(false);
        MatchTickets
            .Setup(x => x.GetAndLockTicketAsync(It.IsAny<MatchTicketClass>(), request.MatchId, It.IsAny<string>()))
            .ReturnsAsync(ticket);
        Payments.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        Payments.Setup(x => x.AddAsync(It.IsAny<Payment>())).ReturnsAsync(new Payment());
    }
}
