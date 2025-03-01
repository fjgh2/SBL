using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBL.Api.Dtos;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<List<Product>>> GetProductsAsync()
    {
        var products = await _productService.GetAllProductsAsync();

        foreach (var product in products)
        {
            if (!string.IsNullOrEmpty(product.Picture))
            {
                product.Picture =
                    $"{Request.Scheme}://{Request.Host}/static/product-images/{product.Id}/{product.Picture}";
            }
        }

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductAsync(int id)
    {
        if (id < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid product id." });
        }

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return BadRequest(new ProblemDetails() { Title = $"No product {id}" });
        }

        if (!string.IsNullOrEmpty(product.Picture))
        {
            product.Picture = $"{Request.Scheme}://{Request.Host}/static/product-images/{product.Id}/{product.Picture}";
        }

        return Ok(product);
    }

    [HttpPost("products/{id}/image")]
    public async Task<IActionResult> UploadProductImage(int id, IFormFile image)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        string fileExtension = Path.GetExtension(image.FileName);
        string fileName = $"{Guid.NewGuid()}{fileExtension}";
        string directoryPath = Path.Combine(
            Directory.GetCurrentDirectory(), "StaticFiles", "product-images", id.ToString());
        Directory.CreateDirectory(directoryPath);
        string filePath = Path.Combine(directoryPath, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        product.Picture = fileName;
        await _productService.UpdateProductAsync(product);

        return Ok(new { path = $"/product-images/{product.Id}/{product.Picture}" });
    }

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProductAsync(CreateProductDto productDto)
    {
        if (productDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid product data" });
        }
        
        var product = _mapper.Map<Product>(productDto);
        int id = await _productService.AddProductAsync(product);
        product.Id = id;

        return Created($"/api/Product/{product.Id}", product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<Product>> UpdateProductAsync(UpdateProductDto productDto)
    {
        if (productDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid product data" });
        }

        var product = await _productService.GetProductByIdAsync(productDto.Id);
        if (product == null)
        {
            return NotFound();
        }

        _mapper.Map(productDto, product);
        await _productService.UpdateProductAsync(product);

        return Ok(product);
    }

    // [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProductAsync(int id)
    {
        if (id < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid product id." });
        }
    
        var product = await _productService.GetProductByIdAsync(id);
        if (product != null && !string.IsNullOrEmpty(product.Picture))
        {
            string imagePath = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "StaticFiles", 
                "product-images", 
                id.ToString(),
                product.Picture);
            
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        await _productService.DeleteProductAsync(id);

        return Ok();
    }
}
