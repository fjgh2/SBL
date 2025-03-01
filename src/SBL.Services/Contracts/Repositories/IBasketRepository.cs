using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Repositories;

public interface IBasketRepository : IGenericRepository<BasketItem>
{
    Task<Result<IEnumerable<BasketItem>>> GetUserBasketAsync(int userId);
}
