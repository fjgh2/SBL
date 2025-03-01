using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Repositories;

public interface IOrderItemRepository : IGenericRepository<OrderItem>
{
    Task<Result<IEnumerable<OrderItem>>> GetOrderItemsAsync(int orderId);
}
