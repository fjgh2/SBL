using Microsoft.Extensions.DependencyInjection;
using SBL.Domain.Common;
using SBL.Domain.Contracts;
using SBL.Services.Auth;
using SBL.Services.Contracts.Services;
using SBL.Services.Ordering;
using SBL.Services.UserManagement;

namespace SBL.Services;

public static class ContainerConfigExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IDateTimeProvider, CurrentDateTimeProvider>();
        services.AddScoped<TokenHelper>();
    }

    public static void RegisterSessionManagement(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<SessionHelper>();
        services.AddDistributedMemoryCache();
    }
}
