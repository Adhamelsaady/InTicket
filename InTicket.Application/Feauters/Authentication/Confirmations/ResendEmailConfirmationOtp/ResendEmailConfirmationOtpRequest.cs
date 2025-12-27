using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Confirmations.ResendEmailConfirmationOtp;

public class ResendEmailConfirmationOtpRequest : IRequest<bool>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}