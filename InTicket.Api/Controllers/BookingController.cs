using System.Security.Claims;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Feauters.Booking.BookTickets;
using InTicket.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/Booking")]
public class BookingController : ControllerBase
{
    private readonly IStripePaymentServices _paymentService;
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public BookingController(IMediator mediator , IStripePaymentServices stripePaymentService , IConfiguration configuration)
    {
        _mediator = mediator;
        _paymentService = stripePaymentService;
        _configuration = configuration;
    }

    [Authorize]
    [HttpPost("{matchId:guid}/book")]
    public async Task<IActionResult> BookTickets(List <MatchTicketForBookingDto> tickets , Guid matchId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
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
        if(bookingTicketsResponse.IsSuccess == false)
            return BadRequest("Failed to book tickets.");
        return Ok(bookingTicketsResponse);
    }
    
    [Authorize]
    [HttpPost("{matchId:guid}/complete_payment")]
    public async Task<IActionResult> CompletePayment([FromBody] CompletePaymentRequest request)
    { 
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var response = await _paymentService.InitiateStripePaymentAsync(request.PaymentCode , User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPost("webhook")]
    [AllowAnonymous] 
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeSignature = Request.Headers["Stripe-Signature"];
            var webhookSecret = _configuration["Stripe:WebhookSecret"];

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                webhookSecret
            );
        
            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Console.WriteLine($"Payment succeeded: {paymentIntent.Id}");
            
                var result = await _paymentService.ConfirmPaymentAsync(paymentIntent.Id);
                Console.WriteLine($"Confirm result: {result}");
            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Console.WriteLine($"Payment failed: {paymentIntent.Id}");
            }
            else if (stripeEvent.Type == "payment_intent.canceled")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Console.WriteLine($"Payment canceled: {paymentIntent.Id}");
            }
            
            return Ok(); 
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Stripe webhook error: {e.Message}");
            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
            return StatusCode(500);
        }
    }
}