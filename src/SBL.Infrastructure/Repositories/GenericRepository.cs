using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly SblDbContext _context;

    public GenericRepository(SblDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<T>>> GetAllAsync()
    {
        try
        {
            var list = await _context.Set<T>().AsNoTracking().ToListAsync();

            return Result.Success((IEnumerable<T>)list);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<T>>($"Error fetching {typeof(T)}: {ex.Message}");
        }
    }

    public async Task<Result<T>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);

            return Result.Success(entity);
        }
        catch (Exception e)
        {
            return Result.Fail<T>(e.Message);
        }
    }

    public async Task<Result<IEnumerable<T>>> GetListByConditionAsync(
        Expression<Func<T, bool>> condition)
    {
        try
        {
            var filteredEntities = await _context.Set<T>()
                .Where(condition)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success<IEnumerable<T>>(filteredEntities);
        }
        catch (Exception e)
        {
            return Result
                .Fail<IEnumerable<T>>($"Failure retrieving data: {e.Message}");
        }
    }

    public async Task<Result<T>> GetByConditionAsync(Expression<Func<T, bool>> condition)
    {
        try
        {
            var item = await _context.Set<T>()
                .Where(condition)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                return Result.Success(item);
            }
            else
            {
                return Result.Fail<T>("Item not found.");
            }
        }
        catch (Exception e)
        {
            return Result.Fail<T>($"Failure retrieving data.: {e.Message}");
        }
    }

    public async Task<T> CreateAsync(T entity)
    {
        var entityEntry = await _context.AddAsync(entity);

        return entityEntry.Entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity != null)
        {
            _context.Remove(entity);
        }
    }

    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Fail($"Error saving changes: {e.Message}");
        }
    }
}