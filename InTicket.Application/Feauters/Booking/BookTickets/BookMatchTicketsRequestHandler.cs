using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTicket.Application.Feauters.Booking.BookTickets;

public class BookMatchTicketsRequestHandler : IRequestHandler<BookMatchTicketsRequest , BookMatchTicketsResponse>
{
    private readonly IDelegationsRepository _delegationsRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMatchTicketRepository _matchTicketRepository;
    
    public BookMatchTicketsRequestHandler(IDelegationsRepository delegationsRepository , 
        IMatchRepository matchRepository ,
        UserManager<ApplicationUser> userManager , 
        IMatchTicketRepository matchTicketRepository)
    {
        _delegationsRepository = delegationsRepository;
        _matchRepository = matchRepository;
        _userManager = userManager;
        _matchTicketRepository = matchTicketRepository;
    }
    
    public async Task<BookMatchTicketsResponse> Handle(BookMatchTicketsRequest request, CancellationToken cancellationToken)
    {
        // home team id
        // away team id
        var match =  await _matchRepository.GetMatchByIdAsync(request.MatchId, false);
        if (match == null)
            return new BookMatchTicketsResponse() {IsSuccess = false};
        if(! await ValidateBooking(request , match) || ! await AnyUserHasBooked(request))  // this validates that the given request is valid in terms of user
            return new BookMatchTicketsResponse() {IsSuccess = false};
        
        return new BookMatchTicketsResponse();
    }

    private async Task<bool> ValidateBooking(BookMatchTicketsRequest request , Match match)
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
            if (request.UserId != booksFor && !await _delegationsRepository.IsDelegatedAsync(booksFor , request.UserId))
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
            if( await _matchTicketRepository.UserHasTicketForMatchAsync(ticket.BookingForUserId , request.MatchId)) 
                return true;
        }

        return false;
    }
    
}