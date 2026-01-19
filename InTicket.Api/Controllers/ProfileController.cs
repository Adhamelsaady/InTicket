using System.Security.Claims;
using InTicket.Application.Feauters.Profile.Commands.AddDelegate;
using InTicket.Application.Feauters.Profile.Queries.GetMyDelegation;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("delegation" , Name =  "GetMyDelegation")]
    [Authorize]
    public async Task<IActionResult> GetMyDelegation()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var getDelegationRequest = new GetMyDelegationsRequest() {currentUserId =  userId};
        var myDelegation = await _mediator.Send(getDelegationRequest);
        if (myDelegation == null)
        {
            return BadRequest("You haven't delegate anyone yet.");
        }
        return Ok(myDelegation);
    }

    [HttpPost("delegation")]
    [Authorize]
    public async Task<IActionResult> Delegate([FromBody]string delegateNationalId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var addDelegateRequest = new AddDelegateRequest()
        {
            DelegatorId = userId,
            DelegateNationalId =  delegateNationalId
        };
        var result = await _mediator.Send(addDelegateRequest);
        if(result == false)
            return BadRequest("National id is wrong or you already delegated someone.");
        return CreatedAtRoute("GetMyDelegation" ,
            new {}
            , result);
    }
    
}