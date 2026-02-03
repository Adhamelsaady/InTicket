using InTicket.Domain.Dtos;
using MediatR;

namespace InTicket.Application.Feauters.Booking.BookTickets;

public class BookMatchTicketsRequest : IRequest<BookMatchTicketsResponse>
{
    public List<MatchTicketForBookingDto> MatchTicketForBookingDtos { get; set; }
    public string UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public Guid MatchId { get; set; }
}