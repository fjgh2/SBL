using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBL.Api.Dtos;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    
    private readonly IMapper _mapper;

    public BasketController(IBasketService basketService,
        IMapper mapper)
    {
        _basketService = basketService;
        _mapper = mapper;
    }
    
    // [Authorize(Roles = "AuthCustomer")]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<List<BasketItem>>> GetBasketAsync(int userId)
    {
        if (userId < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid userId" });
        }
        
        var basket = await _basketService.GetBasketItemsAsync(userId);

        return Ok(basket.ToArray());
    }

    // [Authorize(Roles = "AuthCustomer")]
    [HttpPost]
    public async Task<ActionResult<BasketItem>> AddToBasketAsync(CreateBasketItemDto basketItemDto)
    {
        if (basketItemDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid basket item data" });
        }

        var basketItem = _mapper.Map<BasketItem>(basketItemDto);
        var basketItemId = await _basketService.AddItemToBasketAsync(basketItem);

        return CreatedAtAction(nameof(AddToBasketAsync), 
            new { BasketItemId = basketItemId }, basketItem);
    }
    
    // [Authorize(Roles = "AuthCustomer")]
    [HttpPut("quantity")]
    public async Task<ActionResult> UpdateQuantityAsync(UpdateQuantityDto updateQuantityDto)
    {
        if (updateQuantityDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid quantity data" });
        }

        await _basketService.UpdateQuantityAsync(
            updateQuantityDto.BasketItemId, updateQuantityDto.NewValue);

        return NoContent();
    }

    // [Authorize(Roles = "AuthCustomer")]
    [HttpDelete("{basketItemId:int}")]
    public async Task<ActionResult> DeleteBasketItemAsync(int basketItemId)
    {
        if (basketItemId < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid userId" });
        }
        
        await _basketService.DeleteItemFromBasketAsync(basketItemId);

        return NoContent();
    }
}
