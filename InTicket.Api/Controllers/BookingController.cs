using System.Security.Claims;
using InTicket.Application.Feauters.Booking.BookTickets;
using InTicket.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/Booking")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("{matchId:guid}/book")]
    public async Task<IActionResult> BookTickets(List <MatchTicketForBookingDto> tickets , Guid matchId)
    {
        if (tickets == null || !tickets.Any())
            return BadRequest("No tickets provided.");
        var bookTicketsRequest = new BookMatchTicketsRequest()
        {
            MatchTicketForBookingDtos =  tickets,
            UserId =  User.FindFirstValue(ClaimTypes.NameIdentifier),
            MatchId = matchId,
            BookingDate =  DateTime.Now
        };
        var list = bookTicketsRequest.MatchTicketForBookingDtos;
        if (list.Count > 5)
            return BadRequest("You can book only up to 5 tickets.");

        var bookingTicketsResponse = await _mediator.Send(bookTicketsRequest);
        return Ok(bookingTicketsResponse);
    }
}