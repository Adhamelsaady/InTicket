using System.Linq.Expressions;
using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IBaseRepository <T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByIdAsync(string id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
    Task<ApplicationUser?> GetByNationalIdAsync(string nationalId);
    Task AddRangeAsync(IEnumerable<T> entities);

}