using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Repositories;

public interface ICouponRepository : IGenericRepository<Coupon>
{
    Task<Result<Coupon>> GetCouponByCodeAsync(string couponCode);
}
