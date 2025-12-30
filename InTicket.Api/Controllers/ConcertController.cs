using InTicket.Application.Feauters.Concerts.Queries.GetAllConcerts;
using InTicket.Application.Feauters.Concerts.Queries.GetConcertById;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/Concerts")]
public class ConcertController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConcertController(IMediator mediator) => _mediator = mediator;

    [HttpGet][AllowAnonymous]
    public async Task<IActionResult> GetAllConcerts([FromQuery] ConcertResourceParameters concertResourceParameters)
    {
        var getAllConcertsRequest = new GetAllConcertsRequest() {ConcertResourceParameters = concertResourceParameters};
        var result = await  _mediator.Send(getAllConcertsRequest);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetConcertById(Guid id)
    {
        var getConcertByIdRequest = new GetConcertByIdRequest() { Id = id };
        var concert = await _mediator.Send(getConcertByIdRequest);
        return Ok(concert);
    }
}