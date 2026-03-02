using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
    Task <bool> MarkRefreshTokenAsUsedAsync(RefreshToken refreshToken);
}