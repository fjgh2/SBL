using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Domain.Extensions;
using SBL.Services.Contracts.Repositories;
using SBL.Services.Contracts.Services;

namespace SBL.Services.Ordering;

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<Result<IEnumerable<Coupon>>> GetAllCouponsAsync()
    {
        var result = await _couponRepository.GetAllAsync();
        if (result.Failure)
        {
            return Result.Fail<IEnumerable<Coupon>>(result.Error);
        }

        return Result.Success(result.Value);
    }

    public async Task<int> CreateCouponAsync(Coupon coupon)
    {
        var createdCoupon = await _couponRepository.CreateAsync(coupon);
        var result = await _couponRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception(result.Error));

        return createdCoupon.Id;
    }

    public async Task UpdateCouponAsync(Coupon coupon)
    {
        await _couponRepository.UpdateAsync(coupon);
        var result = await _couponRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception(result.Error));
    }

    public async Task DeleteCouponAsync(int couponId)
    {
        await _couponRepository.DeleteAsync(couponId);
        var result = await _couponRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception(result.Error));
    }

    public async Task<Coupon> ValidateCouponAsync(string code)
    {
        var result = await _couponRepository.GetCouponByCodeAsync(code);
        if (result.Failure && result.Error.Contains("Data access"))
        {
            throw new InvalidOperationException(result.Error);
        }
        else if (result.Value == null)
        {
            throw new ArgumentException(result.Error);
        }

        return result.Value;
    }
}
