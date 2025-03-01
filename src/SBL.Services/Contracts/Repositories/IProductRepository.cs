using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Result<IList<Product>>> GetAllProductsAsync();
    
    Task<Result<IList<Product>>> GetByIdsAsync(int[] ids);
}
