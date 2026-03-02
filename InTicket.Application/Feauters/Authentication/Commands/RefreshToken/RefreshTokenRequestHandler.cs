using InTicket.Domain.Dtos;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.RefreshToken;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest , AuthResult>
{
    public Task<AuthResult> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}