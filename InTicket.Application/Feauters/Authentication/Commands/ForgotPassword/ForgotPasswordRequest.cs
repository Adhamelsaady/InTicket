using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.ForgotPassword;

public class ForgotPasswordRequest : IRequest<bool>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}