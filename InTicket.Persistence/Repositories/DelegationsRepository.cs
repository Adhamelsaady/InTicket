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
    public async Task<Delegation> GetDelegationAsync(string delegatorId)
    {
        var query = _dbContext.Delegations.AsQueryable();
        return await query.FirstOrDefaultAsync(d => d.DelegatorId == delegatorId);
    }

    public async Task<bool> HasDelegationAsync(string delegatorId)
    {
        var query = _dbContext.Delegations.AsQueryable();
        return await query.AnyAsync(d => d.DelegatorId == delegatorId);
    }

    public async Task<Delegation> AddAsync(Delegation delegation)
    {
        await _dbContext.Delegations.AddAsync(delegation);
        await  _dbContext.SaveChangesAsync();
        return delegation;
    }
}