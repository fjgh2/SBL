using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SBL.Domain.Common;
using SBL.Services.Contracts;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure;

public class OrderUnitOfWork : IUnitOfWork
{
    private readonly SblDbContext _context;
    
    private IDbContextTransaction _transaction;
    
    private bool _disposed;

    public OrderUnitOfWork(
        SblDbContext context,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IProductRepository productRepository,
        ICouponRepository couponRepository)
    {
        _context = context;
        OrderRepository = orderRepository;
        OrderItemRepository = orderItemRepository;
        ProductRepository = productRepository;
        CouponRepository = couponRepository;
    }

    public IOrderRepository OrderRepository { get; }
    
    public IOrderItemRepository OrderItemRepository { get; }
    
    public IProductRepository ProductRepository { get; }
    
    public ICouponRepository CouponRepository { get; }
    
    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public async Task<Result> CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            _transaction.Commit();
    
            return Result.Success();
        }
        catch (Exception ex)
        {
            RollbackTransaction();
            
            return Result.Fail($"Transaction failed: {ex.Message}");
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            
            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail($"Database update error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error saving changes: {ex.Message}");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        
        _disposed = true;
    }
}
