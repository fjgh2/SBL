using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Services;

public interface ICouponService
{
    Task<Result<IEnumerable<Coupon>>> GetAllCouponsAsync(); 
    
    Task<int> CreateCouponAsync(Coupon coupon);
    
    Task UpdateCouponAsync(Coupon coupon);
    
    Task DeleteCouponAsync(int couponId);
    
    Task<Coupon> ValidateCouponAsync(string code);
}
