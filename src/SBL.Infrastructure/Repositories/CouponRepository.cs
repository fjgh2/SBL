using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
{
    public CouponRepository(SblDbContext context) : base(context)
    {
    }

    public async Task<Result<Coupon>> GetCouponByCodeAsync(string couponCode)
    {
        try
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == couponCode);

            return Result.Success(coupon);
        }
        catch (Exception e)
        {
            return Result.Fail<Coupon>($"Data access error: {e.Message}");
        }
    }
}
