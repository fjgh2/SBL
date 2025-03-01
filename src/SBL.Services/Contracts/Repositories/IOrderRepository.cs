using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Result<IEnumerable<Order>>> GetOrdersOfUser(int userId);
    
    Task<Result<Order>> GetOrderAsync(int orderId);
    
    Task<Result<IEnumerable<Order>>> GetAllOrders();
}
