using System.ComponentModel.DataAnnotations;
using InTicket.Domain.Dtos;
using MediatR;

namespace InTicket.Application.Feauters.Booking.BookTickets;

public class BookMatchTicketsRequest : IRequest<BookMatchTicketsResponse>
{
    [Required] public List<MatchTicketForBookingDto> MatchTicketForBookingDtos { get; set; }
    [Required] public string UserId { get; set; }
    [Required] public DateTime BookingDate { get; set; }
    [Required] public Guid MatchId { get; set; }
}