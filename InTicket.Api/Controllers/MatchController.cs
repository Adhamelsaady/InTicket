using InTicket.Application.Feauters.Matches.Commands.ActivateMatch;
using InTicket.Application.Feauters.Matches.Commands.CreateMatch;
using InTicket.Application.Feauters.Matches.Commands.DeleteMatch;
using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;
using InTicket.Application.ResourceParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public MatchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("id" , Name =  "GetMatchById")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMatchById(Guid id)
    {
        bool isAdmin = User.IsInRole("Admin");
        var getMatchByIdRequest = new GetMatchByIdRequest() { Id = id , IsRequestedByAdmin = isAdmin };
        var result = await _mediator.Send(getMatchByIdRequest);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet][AllowAnonymous]
    public async Task<IActionResult> GetAllMatches([FromQuery] MatchResourceParameters matchResourceParameters)
    {
        bool isAdmin = User.IsInRole("Admin");
        var getAllMatchesRequest =  new GetAllMatchesRequest() { MatchResourceParameters = matchResourceParameters , IsRequestedByAdmin = isAdmin};
        var result = await _mediator.Send(getAllMatchesRequest);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest createMatchRequest)
    {
        if (!ModelState.IsValid)
        {
            return  BadRequest(ModelState);
        }        
        var result = await _mediator.Send(createMatchRequest);
       
        return CreatedAtRoute(
            "GetMatchById", 
            new { id = result.Id },
            result);
    }

    [HttpPut("/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActivateMatchBooking(Guid id)
    {
        var activateMatchRequest = new ActivateMatchRequest() { Id = id };
        var result = await _mediator.Send(activateMatchRequest);
        if(! result) return BadRequest();
        return Ok(new { message = "Match activated successfully", matchId = id });
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]  
    public async Task<IActionResult> DeleteMatch(Guid id)
    {
        var request = new DeleteMatchRequest { Id = id };
        var result = await _mediator.Send(request);

        if (!result)
        {
            return NotFound(new { message = $"Match with ID {id} not found" });
        }

        return NoContent();
    }
    
}