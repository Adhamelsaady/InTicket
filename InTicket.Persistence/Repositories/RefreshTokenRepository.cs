using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly InTicketDbContext _dbContext;
    public RefreshTokenRepository(InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
    {
        refreshToken = refreshToken.ToLower();
        var refreshTokenEntity = await _dbContext.RefreshTokens.Where(t => refreshToken == t.Token.ToLower())
            .Include(t => t.User).FirstOrDefaultAsync();
        return refreshTokenEntity!;
    }
    public async Task <bool> MarkRefreshTokenAsUsedAsync(RefreshToken refreshToken)
    {
        refreshToken.isUsed = true;
        return await _dbContext.SaveChangesAsync() > 0; 
    }
    public async Task <bool> MarkRefreshTokenAsRevokedAsync(string refreshToken)
    {
        var refreshTokenEntity = await GetRefreshTokenAsync(refreshToken);
        refreshTokenEntity.isUsed = true;
        return await _dbContext.SaveChangesAsync() > 0; 
    }
}