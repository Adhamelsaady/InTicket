using InTicket.Domain.Dtos;

namespace InTicket.Application.Contracts.Infrasructure;

public interface IStripePaymentServices 
{
    Task<CompletePaymentResponse> InitiateStripePaymentAsync(Guid paymentCode);
    Task<bool> ConfirmPaymentAsync(string paymentIntentId);
}