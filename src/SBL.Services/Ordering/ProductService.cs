using SBL.Domain.Entities;
using SBL.Domain.Extensions;
using SBL.Services.Contracts.Repositories;
using SBL.Services.Contracts.Services;

namespace SBL.Services.Ordering;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var result = await _productRepository.GetAllProductsAsync();
        result.OnFailure(() => throw new Exception("Failure getting products"));

        return result.Value;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var result = await _productRepository.GetByIdAsync(id);
        result.OnFailure(() => throw new Exception("Failure getting products"));

        return result.Value;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        var createdProduct = await _productRepository.CreateAsync(product);
        var result = await _productRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception("Failure adding a new product"));
        
        return createdProduct.Id;
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateAsync(product);
        var result = await _productRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception("Failure updating a product"));
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
        var result = await _productRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception("Failure deleting a product"));
    }
}
