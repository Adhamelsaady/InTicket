using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using InTicket.Persistence;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace InTicket.Infrastructure.Stripe;

public class StripePaymentService : IStripePaymentServices
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IConfiguration _configuration;

    public StripePaymentService(IPaymentRepository paymentRepository, IConfiguration configuration)
    {
        _paymentRepository = paymentRepository;
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }
    
    public async Task<CompletePaymentResponse> InitiateStripePaymentAsync(Guid paymentCode)
    {
        // Find payment by code
        var payment = await _paymentRepository.GetPaymentByPaymentCodeAsync(paymentCode);
        if (payment == null)
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
            Amount = (long)(payment.Price * 100), // Stripe uses cents
            Currency = "usd",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
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
        payment.Done = true;

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
            return false;
        payment.Done = true;
        return true;
    }

}