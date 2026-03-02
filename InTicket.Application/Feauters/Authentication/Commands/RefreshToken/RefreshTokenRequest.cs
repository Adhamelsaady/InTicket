using System.ComponentModel.DataAnnotations;
using InTicket.Domain.Dtos;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.RefreshToken;

public class RefreshTokenRequest : IRequest<AuthResult>
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}