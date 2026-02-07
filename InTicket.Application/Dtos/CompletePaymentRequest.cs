using System.ComponentModel.DataAnnotations;

namespace InTicket.Domain.Dtos;

public class CompletePaymentRequest
{
    [Required]
    public Guid PaymentCode { get; set; }
}