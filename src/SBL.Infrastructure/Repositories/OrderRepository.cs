using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(SblDbContext context) : base(context)
    {
    }

    public async Task<Result<IEnumerable<Order>>> GetOrdersOfUser(int userId)
    {
        try
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Include(o => o.Coupon)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success<IEnumerable<Order>>(orders);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<Order>>($"Error fetching orders: {e.Message}");
        }
    }

    public async Task<Result<Order>> GetOrderAsync(int orderId)
    {
        try
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.Coupon)
                .FirstOrDefaultAsync();
            
            return Result.Success(order);
        }
        catch (Exception e)
        {
            return Result.Fail<Order>($"Error fetching order {orderId}: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Order>>> GetAllOrders()
    {
        try
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Include(o => o.Coupon)
                .AsNoTracking()
                .ToListAsync();
            
            return Result.Success((IEnumerable<Order>) orders);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Order>>($"Error fetching orders: {ex.Message}");
        }
    }
}
