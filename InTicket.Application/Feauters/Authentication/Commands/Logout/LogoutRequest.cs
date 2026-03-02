using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.Logout;

public class LogoutRequest : IRequest<bool>
{
    [Required]
    public string RefreshToken { get; set; }
}