using System.Security.Claims;
using InTicket.Application.Feauters.Booking.BookTickets;
using InTicket.Domain.Dtos;
using MediatR;
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

    [HttpPost ("booktickets")]
    public async Task<IActionResult> BookTickets(List <MatchTicketForBookingDto> matchTicketForBookingDtos , Guid matchId)
    {
        var bookTicketsRequest = new BookMatchTicketsRequest()
        {
            MatchTicketForBookingDtos =  matchTicketForBookingDtos,
            UserId =  User.FindFirstValue(ClaimTypes.NameIdentifier),
            MatchId = matchId,
            BookingDate =  DateTime.Now
        };
        var list = bookTicketsRequest.MatchTicketForBookingDtos;
        if (list.Count > 5)
            return BadRequest("You can book only up to 5 tickets.");

        var paymentCode = await _mediator.Send(bookTicketsRequest);
        return Ok();
    }
}