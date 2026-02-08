using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using InTicket.Application.Feauters.Profile.Commands.AddDelegate;
using InTicket.Application.Feauters.Profile.Commands.DeleteDelegation;
using InTicket.Application.Feauters.Profile.Queries.GetMyDelegation;
using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using InTicket.Application.Feauters.Profile.Queries.GetPayment;
using InTicket.Application.ResourceParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("delegation", Name = "GetMyDelegation")]
    [Authorize]
    public async Task<IActionResult> GetMyDelegation()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var getDelegationRequest = new GetMyDelegationsRequest() { currentUserId = userId };
        var myDelegation = await _mediator.Send(getDelegationRequest);
        if (myDelegation == null)
        {
            return Ok("You haven't delegate anyone yet.");
        }

        return Ok(myDelegation);
    }

    [HttpPost("delegation")]
    [Authorize]
    public async Task<IActionResult> Delegate([FromBody] [Required] [Length(14, 14)] DelegationForCreateDto delegationForCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var addDelegateRequest = new AddDelegateRequest()
        {
            DelegatorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            DelegateNationalId = delegationForCreateDto.DelegateNationalId
        };
        var result = await _mediator.Send(addDelegateRequest);
        if (result == false)
            return BadRequest("National id is wrong or you already delegated someone.");
        return CreatedAtRoute("GetMyDelegation",
            new { }
            , $"Your delegation to user with national id {delegationForCreateDto.DelegateNationalId} has been created.");
    }

    [HttpDelete("delegation/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteDelegation([Required] Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var deleteDelegationRequest = new DeleteDelegationRequest()
        {
            DelegatorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            DelegationId = id
        };
        var result = await _mediator.Send(deleteDelegationRequest);
        if (!result) return BadRequest("Delegation not found.");
        return Ok();
    }
    
    [HttpGet("payments")]
    [Authorize]
    public async Task<IActionResult> GetPayments([FromQuery] PaymentResourceParameters paymentResourceParameters)
    {
        var getPaymentsRequest = new GetPaymentsRequest()
        {
            PaymentResourceParameters = paymentResourceParameters,
            UserId =  User.FindFirstValue(ClaimTypes.NameIdentifier)
        };
        var result = await _mediator.Send(getPaymentsRequest);
        return Ok(result);
    }

    [HttpGet("payments/{paymentId}")]
    [Authorize]
    public async Task<IActionResult> GetPayment([FromRoute] Guid paymentId)
    {
        var getPaymentRequest = new GetPaymentRequest() { PaymentId = paymentId , UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) };
        var result = await _mediator.Send(getPaymentRequest);
        return Ok(result);
    }
}