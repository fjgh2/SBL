using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBL.Api.Dtos;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;

    private readonly IMapper _mapper;

    public CouponController(ICouponService couponService, IMapper mapper)
    {
        _couponService = couponService;
        _mapper = mapper;
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet(Name = "all")]
    public async Task<ActionResult<List<Coupon>>> GetCouponsAsync()
    {
        var result = await _couponService.GetAllCouponsAsync();
        if (!result.Failure)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateCouponAsync(CreateCouponDto couponDto)
    {
        if (couponDto == null)
        {
            return BadRequest("Invalid coupon data");
        }

        var coupon = _mapper.Map<Coupon>(couponDto);
        var couponId = await _couponService.CreateCouponAsync(coupon);

        return Ok(couponId);
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<Coupon>> UpdateCouponAsync(UpdateCouponDto couponDto)
    {
        if (couponDto == null)
        {
            return BadRequest("Invalid coupon data");
        }

        var coupon = _mapper.Map<Coupon>(couponDto);
        await _couponService.UpdateCouponAsync(coupon);

        return Ok(coupon);
    }

    // [Authorize(Roles = "Admin")]
    [HttpDelete("{couponId:int}")]
    public async Task<ActionResult> DeleteCouponAsync(int couponId)
    {
        if (couponId < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid coupon id" });
        }

        await _couponService.DeleteCouponAsync(couponId);

        return NoContent();
    }
}
