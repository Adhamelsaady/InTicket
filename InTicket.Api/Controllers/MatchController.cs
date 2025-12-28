using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;
using InTicket.Application.ResourceParameters;
using MediatR;
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

    [HttpGet("id")]
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

    [HttpGet]
    public async Task<IActionResult> GetAllMatches([FromQuery] MatchResourceParameters matchResourceParameters)
    {
        var getAllMatchesRequest =  new GetAllMatchesRequest() { MatchResourceParameters = matchResourceParameters };
        var result = await _mediator.Send(getAllMatchesRequest);
        return Ok(result);
    }
}