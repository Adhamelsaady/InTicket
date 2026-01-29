using InTicket.Application.Feauters.Concerts.ActivateConcert;
using InTicket.Application.Feauters.Concerts.Commands.CreateConcert;
using InTicket.Application.Feauters.Concerts.Commands.DeleteConcert;
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

    [HttpGet(Name = "GetAllConcerts")][AllowAnonymous]
    public async Task<IActionResult> GetAllConcerts([FromQuery] ConcertResourceParameters concertResourceParameters)
    { 
        bool isAdmin = User.IsInRole("Admin");
        var getAllConcertsRequest = new GetAllConcertsRequest() {ConcertResourceParameters = concertResourceParameters ,  IsRequestedByAdmin = isAdmin};
        var result = await  _mediator.Send(getAllConcertsRequest);
        return Ok(result);
    }

    [HttpGet("{id}" , Name = "GetConcertById")]
    public async Task<IActionResult> GetConcertById(Guid id)
    {
        bool isAdmin = User.IsInRole("Admin");
        var getConcertByIdRequest = new GetConcertByIdRequest() { Id = id ,  IsRequestedByAdmin = isAdmin };
        var concert = await _mediator.Send(getConcertByIdRequest);
        return Ok(concert);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateConcert([FromBody] CreateConcertRequest concertRequest)
    {
        if (!ModelState.IsValid)
        {
            return  BadRequest(ModelState);
        }        
        var result = await _mediator.Send(concertRequest);
       
        return CreatedAtRoute(
            "GetConcertById", 
            new { id = result.Id },
            result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> ActivateConcert([FromRoute] Guid id)
    {
        var activateConcertRequest = new ActivateConcertRequest() { Id = id };
        var result = await _mediator.Send(activateConcertRequest);
        if (!result) return BadRequest();
        return Ok(new { message = "Concert activated successfully", matchId = id });
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConcert(Guid id)
    {
        var deleteRequest = new DeleteConcertRequest() { Id = id };
        var result = await _mediator.Send(deleteRequest);

        if (!result)
        {
            return NotFound(new { message = $"Concert with ID {id} not found" });
        }

        return NoContent();
    }
}