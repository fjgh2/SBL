using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Services;

public interface IBasketService
{
    Task<int> AddItemToBasketAsync(BasketItem basketItem);
    
    Task UpdateQuantityAsync(int basketItemId, int newValue);
    
    Task DeleteItemFromBasketAsync(int basketItemId);
    
    Task<IEnumerable<BasketItem>> GetBasketItemsAsync(int userId);
    
    Task<decimal> CalculateTotal(int userId);
}
