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
        var getMatchByIdRequest = new GetMatchByIdRequest() { Id = id };
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
        var getAllMatchesRequest =  new GetAllMatchesRequest() { MatchResourceParameters = matchResourceParameters };
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]  
    public async Task<IActionResult> DeleteMatch(Guid id)
    {
        // This is not working as intended 
        var request = new DeleteMatchRequest { Id = id };
        var result = await _mediator.Send(request);

        if (!result)
        {
            return NotFound(new { message = $"Match with ID {id} not found" });
        }

        return NoContent();
    }
    
}