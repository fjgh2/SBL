using SBL.Domain.Common;
using SBL.Services.Contracts.Repositories;

namespace SBL.Services.Contracts;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository OrderRepository { get; }

    IOrderItemRepository OrderItemRepository { get; }

    IProductRepository ProductRepository { get; }

    ICouponRepository CouponRepository { get; }

    // IUserRepository UserRepository { get; }
    
    Task<Result> SaveChangesAsync();

    void BeginTransaction();

    Task<Result> CommitTransactionAsync();

    void RollbackTransaction();
}
