using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InTicket.Persistence.Repositories;

public class DelegationsRepository  : IDelegationsRepository
{
    private readonly InTicketDbContext _dbContext;

    public DelegationsRepository(InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Delegation> GetDelegation(string delegatorId)
    {
        var query = _dbContext.Delegations.AsQueryable();
        return await query.FirstOrDefaultAsync(d => d.DelegatorId == delegatorId);
    }
}