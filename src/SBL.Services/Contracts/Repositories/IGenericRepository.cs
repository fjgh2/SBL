using SBL.Domain.Common;

namespace SBL.Services.Contracts.Repositories;

public interface IGenericRepository<T>
{
    Task<Result<IEnumerable<T>>> GetAllAsync();
    
    Task<Result<T>> GetByIdAsync(int id);
    
    Task<T> CreateAsync(T entity);
    
    Task UpdateAsync(T entity);
    
    Task DeleteAsync(int id);

    Task<Result> SaveChangesAsync();
}
