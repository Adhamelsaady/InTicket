using System.Security.Claims;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using InTicket.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace InTicket.Infrastructure.Stripe;

public class StripePaymentService : IStripePaymentServices
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IConfiguration _configuration;
    private readonly IMatchTicketRepository _matchTicketRepository;

    public StripePaymentService(IPaymentRepository paymentRepository, IConfiguration configuration , 
        IMatchTicketRepository matchTicketRepository)
    {
        _paymentRepository = paymentRepository;
        _configuration = configuration;
        _matchTicketRepository = matchTicketRepository;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }
    
    public async Task<CompletePaymentResponse> InitiateStripePaymentAsync(Guid paymentCode , string userId)
    {
        var payment = await _paymentRepository.GetPaymentByPaymentCodeAsync(paymentCode);
        if (payment == null || payment.UserId != userId)
            throw new Exception("Payment not found");
        if (payment.ExpirationDate < DateTime.UtcNow)
        {
            payment.Done = false;
            throw new Exception("Payment code has expired");
        }
        if (payment.Done)
            throw new Exception("Payment already processed");

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(payment.Price * 100), 
            Currency = "usd",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
                AllowRedirects = "never" 
            },
            Metadata = new Dictionary<string, string>
            {
                { "paymentCode", payment.PaymentId.ToString() },
                { "userId", payment.UserId }
            }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);
        payment.PaymentIntentId = paymentIntent.Id;
        payment.Done = false; 
        await _paymentRepository.UpdateAsync(payment); 
        return new CompletePaymentResponse
        {
            ClientSecret = paymentIntent.ClientSecret,
            PaymentIntentId = paymentIntent.Id,
            Amount = payment.Price
        };
    }
    

    public async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
    {
        var payment = await _paymentRepository.GetPaymentByIntentAsync(paymentIntentId);
        if (payment == null)
        {
            return false;
        }
        payment.Done = true;
        await _paymentRepository.UpdateAsync(payment);
        foreach (var ticketId in payment.TicketIds)
        {
            await _matchTicketRepository.ChangeTicKetStatus(ticketId, TicketStatus.Booked);
        }
        
        return true;
    }
}