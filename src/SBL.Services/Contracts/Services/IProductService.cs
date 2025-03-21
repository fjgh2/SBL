using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    
    Task<Product> GetProductByIdAsync(int id);
    
    Task<int> AddProductAsync(Product product);
    
    Task UpdateProductAsync(Product product);
    
    Task DeleteProductAsync(int id);
}
