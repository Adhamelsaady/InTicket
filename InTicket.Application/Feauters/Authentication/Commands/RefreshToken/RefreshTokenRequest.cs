using System.ComponentModel.DataAnnotations;
using InTicket.Domain.Dtos;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.RefreshToken;

public class RefreshTokenRequest : IRequest<AuthResult>
{
    [Required]
    string Token { get; set; }
    [Required]
    string RefreshToken { get; set; }
}