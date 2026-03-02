using InTicket.Application.Contracts.Presistance;
using MediatR;

namespace InTicket.Application.Feauters.Authentication.Commands.Logout;

public class LogoutRequestHandler : IRequestHandler<LogoutRequest , bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutRequestHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await _refreshTokenRepository.MarkRefreshTokenAsRevokedAsync(request.RefreshToken);
        return result;
    }
}