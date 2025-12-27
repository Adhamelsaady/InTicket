using System.ComponentModel.DataAnnotations;
using InTicket.Application.Feauters.Authentication.Register;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Login;

public class LoginCommand : IRequest<AuthenticationResponse>
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}