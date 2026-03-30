using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class MatchTicketRepository : IMatchTicketRepository
{
    private readonly InTicketDbContext _dbContext;

    public MatchTicketRepository(InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> UserHasTicketForMatchAsync(string userId, Guid matchId)
    {
        var tickets = _dbContext.MatchTickets.AsQueryable();
        var ticket = await tickets.FirstOrDefaultAsync(t => t.MatchId == matchId && t.HolderId == userId);
        if (ticket == null) return false;
        if (ticket.HeldExpiresAt < DateTime.UtcNow) return false;
        return true;
    }

    public async Task<MatchTicket?> GetAndLockTicketAsync(
        MatchTicketClass matchTicketClass, Guid matchId, string userId)
    {
        // SqlQueryRaw<Guid> expects the column to be named "Value"
        var ticketId = await _dbContext.Database
            .SqlQueryRaw<Guid>(@"
            SELECT TOP 1 TicketId AS Value
            FROM MatchTickets WITH (UPDLOCK, ROWLOCK)
            WHERE MatchId = {0}
              AND TicketClass = {1}
              AND (
                    Status = {2}
                    OR (Status = {3} AND HeldExpiresAt < GETUTCDATE())
                  )",
                matchId,
                (int)matchTicketClass,
                (int)TicketStatus.Open,
                (int)TicketStatus.Held)
            .FirstOrDefaultAsync();

        if (ticketId == Guid.Empty)
            return null;

        return await _dbContext.MatchTickets
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }
    public async Task<bool> UserHasActiveTicketForMatchAsync(string userId, Guid matchId)
    {
        return await _dbContext.MatchTickets
            .AnyAsync(t =>
                t.MatchId == matchId &&
                t.HolderId == userId &&
                (t.Status == TicketStatus.Held || t.Status == TicketStatus.Booked) &&
                t.HeldExpiresAt > DateTime.UtcNow);
    }
    public async Task ChangeTicKetStatus(Guid TicketId, TicketStatus ticketStatus)
    {
        var ticket = _dbContext.MatchTickets.FirstOrDefault(t => t.TicketId == TicketId);
        ticket.Status = ticketStatus;
        await _dbContext.SaveChangesAsync();
    }
    
}