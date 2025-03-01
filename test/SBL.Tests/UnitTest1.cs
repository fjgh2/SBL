using Moq;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;
using SBL.Services.Ordering;

namespace SBL.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnProducts_WhenSuccessful()
    {
        // Arrange
        var products = new List<Product> { new() { Id = 1, Name = "Test" } };
        _productRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(Result.Success((IEnumerable<Product>)products));

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldThrowException_WhenFails()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(Result.Fail<IEnumerable<Product>>("Error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _productService.GetAllProductsAsync());
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test" };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(Result.Success(product));

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldSucceed_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test" };
        _productRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(Result.Success());

        // Act & Assert
        await _productService.UpdateProductAsync(product);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldSucceed_WhenProductExists()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(Result.Success());

        // Act & Assert
        await _productService.DeleteProductAsync(1);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldThrowException_WhenSaveChangesFails()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(Result.Fail("Error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _productService.DeleteProductAsync(1));
    }
}
