using SBL.Domain.Entities;
using SBL.Domain.Extensions;
using SBL.Services.Contracts.Repositories;
using SBL.Services.Contracts.Services;

namespace SBL.Services.Ordering;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;

    public BasketService(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<IEnumerable<BasketItem>> GetBasketItemsAsync(int userId)
    {
        var result = await _basketRepository.GetUserBasketAsync(userId);
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }
    
    public async Task<int> AddItemToBasketAsync(BasketItem basketItem)
    {
        if (basketItem == null)
        {
            throw new ArgumentNullException(nameof(basketItem));
        }
        
        var createdItem = await _basketRepository.CreateAsync(basketItem);
        var result = await _basketRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception(result.Error));

        return createdItem.Id;
    }

    public async Task UpdateQuantityAsync(int basketItemId, int newValue)
    {
        if (newValue < 1)
        {
            throw new ArgumentException("Quantity can't be negative or 0", nameof(newValue));
        }
        
        var result = await _basketRepository.GetByIdAsync(basketItemId);
        result.OnFailure(() => throw new Exception(result.Error));
        if (result.Value == null)
        {
            throw new Exception($"Basket item with ID {basketItemId} not found.");
        }
        
        result.Value.Quantity = newValue;
        await _basketRepository.UpdateAsync(result.Value);
        await _basketRepository.SaveChangesAsync();
    }

    public async Task DeleteItemFromBasketAsync(int basketItemId)
    {
        await _basketRepository.DeleteAsync(basketItemId);
        var result = await _basketRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception(result.Error));
    }

    public async Task<decimal> CalculateTotal(int userId)
    {
        var result = await _basketRepository.GetUserBasketAsync(userId);
        result.OnFailure(() => throw new InvalidOperationException(result.Error));

        return result.Value?.Sum(bi => bi.Total) ?? 0;
    }
}
