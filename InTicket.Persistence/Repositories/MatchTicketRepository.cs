using InTicket.Application.Contracts.Presistance;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class MatchTicketRepository : IMatchTicketRepository
{
    private readonly InTicketDbContext _dbContext;
    
    MatchTicketRepository (InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> UserHasTicketForMatchAsync(string userId, Guid matchId)
    {
        var tickets = _dbContext.MatchTickets.AsQueryable();
        bool exist = await tickets.FirstOrDefaultAsync(t => t.MatchId == matchId && t.HolderId == userId) != null;
        return exist;
    }
    
}