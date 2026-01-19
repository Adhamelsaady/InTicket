using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly InTicketDbContext _dbContext;
    public BaseRepository(InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    { 
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await  SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<ApplicationUser?> GetByNationalIdAsync(string nationalId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.NationalId == nationalId);
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}