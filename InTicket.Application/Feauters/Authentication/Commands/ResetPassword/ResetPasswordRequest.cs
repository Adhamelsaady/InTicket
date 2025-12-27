using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.ResetPassword;

public class ResetPasswordRequest : IRequest<bool>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Otp { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}