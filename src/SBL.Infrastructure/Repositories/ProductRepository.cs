using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(SblDbContext context) : base(context)
    {
    }

    public async Task<Result<IList<Product>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _context.Products
                .Include(p=> p.Tags)
                .Include(p => p.Feedbacks)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success((IList<Product>)products);
        }
        catch (Exception e)
        {
            return Result.Fail<IList<Product>>($"Failure getting products: {e.Message}.");
        }
        
    }
    
    public async Task<Result<IList<Product>>> GetByIdsAsync(int[] ids)
    {
        try
        {
            var products = await _context.Products
                .FromSqlRaw("SELECT * FROM product WHERE id = ANY(@p0)", ids)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success((IList<Product>)products);
        }
        catch (Exception e)
        {
            return Result.Fail<IList<Product>>($"Failure getting products by ids: {e.Message}.");
        }
    }
}
