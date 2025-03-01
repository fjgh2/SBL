using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(SblDbContext context) : base(context)
    {
    }
    
    public async Task<Result<IEnumerable<OrderItem>>> GetOrderItemsAsync(int orderId)
    {
        try
        {
            var orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success<IEnumerable<OrderItem>>(orderItems);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<OrderItem>>(
                    $"Problem getting order items of order {orderId}: {e.Message}");
        }
    }
}
