using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTicket.Application.Feauters.Booking.BookTickets;

public class BookMatchTicketsRequestHandler : IRequestHandler<BookMatchTicketsRequest, BookMatchTicketsResponse>
{
    private readonly IDelegationsRepository _delegationsRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMatchTicketRepository _matchTicketRepository;
    private readonly IBaseRepository<Payment> _paymentRepository;

    public BookMatchTicketsRequestHandler(IDelegationsRepository delegationsRepository,
        IMatchRepository matchRepository,
        UserManager<ApplicationUser> userManager,
        IMatchTicketRepository matchTicketRepository,
        IBaseRepository<Payment> paymentRepository)
    {
        _delegationsRepository = delegationsRepository;
        _matchRepository = matchRepository;
        _userManager = userManager;
        _matchTicketRepository = matchTicketRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<BookMatchTicketsResponse> Handle(BookMatchTicketsRequest request,
        CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetMatchByIdAsync(request.MatchId, false);
        if (match == null)
            return new BookMatchTicketsResponse() { IsSuccess = false };
        if (!await ValidateBooking(request, match) || await AnyUserHasBooked(request))
            return new BookMatchTicketsResponse() { IsSuccess = false };
        await using var transaction = await _paymentRepository.BeginTransactionAsync();
        try
        {
            if (await AnyUserHasBooked(request))
                return new BookMatchTicketsResponse() { IsSuccess = false };
            var tickets = await HoldTheTickets(request);
            if (tickets.Contains(null) || tickets.Count == 0)
                return new BookMatchTicketsResponse() { IsSuccess = false };
            await _paymentRepository.SaveChangesAsync();
            var payment = new Payment()
            {
                PaymentId = Guid.NewGuid(),
                UserId = request.UserId,
                Price = tickets.Sum(t => t.Price),
                ExpirationDate = tickets[0].HeldExpiresAt.Value,
                TicketIds = tickets.Select(t => t.TicketId).ToList(),
                Done = false
            };
            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return new BookMatchTicketsResponse()
            {
                IsSuccess = true,
                PaymentCode = payment.PaymentId,
                TotalPrice = payment.Price,
                TotalTickets = tickets.Count,
                ExpirationDate = payment.ExpirationDate
            };
        } 
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new BookMatchTicketsResponse() { IsSuccess = false };
        }
    }

    private async Task<bool> ValidateBooking(BookMatchTicketsRequest request, Match match)
    {
        var ticketsList = request.MatchTicketForBookingDtos;

        HashSet<string> BookingForIds = new HashSet<string>();
        for (int i = 0; i < ticketsList.Count; ++i)
        {
            var booksFor = ticketsList[i].BookingForUserId;
            if (BookingForIds.Contains(booksFor))
            {
                return false;
            }

            if (request.UserId != booksFor && !await _delegationsRepository.IsDelegatedAsync(booksFor, request.UserId))
            {
                return false;
            }

            var teamId = ticketsList[i].isHomeTeam ? match.HomeTeamId : match.AwayTeamId;
            var user = await _userManager.FindByIdAsync(booksFor);
            if (teamId != user.FavoriteTeamId && match.GeneralBookingStart > DateTime.Now)
            {
                return false;
            }

            BookingForIds.Add(booksFor);
        }

        return true;
    }

    private async Task<bool> AnyUserHasBooked(BookMatchTicketsRequest request)
    {
        foreach (var ticket in request.MatchTicketForBookingDtos)
        {
            if (await _matchTicketRepository.UserHasTicketForMatchAsync(ticket.BookingForUserId, request.MatchId))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<IList<Ticket>> HoldTheTickets(BookMatchTicketsRequest request)
    {
        var result = new List<Ticket>();
        foreach (var ticketForBooking in request.MatchTicketForBookingDtos)
        {
            var ticketToAdd =
                await _matchTicketRepository.GetAndLockTicketAsync(ticketForBooking.Class, request.MatchId,
                    ticketForBooking.BookingForUserId);
            ticketToAdd.HolderId = ticketForBooking.BookingForUserId;
            ticketToAdd.Status = TicketStatus.Held;
            ticketToAdd.HeldExpiresAt = DateTime.UtcNow.AddHours(1);
            result.Add(ticketToAdd);    
        }
        return result;
    }
}