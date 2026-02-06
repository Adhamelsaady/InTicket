namespace InTicket.Domain.Dtos;

public class CompletePaymentResponse
{
    public string ClientSecret { get; set; } // For Stripe frontend
    public string PaymentIntentId { get; set; }
    public decimal Amount { get; set; }
}