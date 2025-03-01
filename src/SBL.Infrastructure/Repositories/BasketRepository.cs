using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class BasketRepository : GenericRepository<BasketItem>, IBasketRepository
{
    public BasketRepository(SblDbContext context) : base(context)
    {
    }

    public async Task<Result<IEnumerable<BasketItem>>> GetUserBasketAsync(int userId)
    {
        try
        {
            var basket = await _context.BasketItems
                .Where(bi => bi.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
            
            return Result.Success<IEnumerable<BasketItem>>(basket);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<BasketItem>>(
                $"Error fetching basket of user {userId}: {e.Message}");
        }
    }
}
