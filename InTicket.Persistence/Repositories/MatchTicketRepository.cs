using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class MatchTicketRepository : IMatchTicketRepository
{
    private readonly InTicketDbContext _dbContext;
    
    public MatchTicketRepository (InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> UserHasTicketForMatchAsync(string userId, Guid matchId)
    {
        var tickets = _dbContext.MatchTickets.AsQueryable();
        bool exist = await tickets.FirstOrDefaultAsync(t => t.MatchId == matchId && t.HolderId == userId) != null;
        return exist;
    }

    public async Task<MatchTicket> GetRandomTicketAsync(MatchTicketClass matchTicketClass , Guid matchId , string UserId)
    {
        var tickets = _dbContext.MatchTickets.AsQueryable();
        var ticket = await tickets.FirstOrDefaultAsync(t => t.MatchId == matchId &&
                                                            (t.Status == TicketStatus.Open ||
                                                             t.HeldExpiresAt < DateTime.Now));
        ticket.HolderId = UserId;
        return ticket;
    }
}