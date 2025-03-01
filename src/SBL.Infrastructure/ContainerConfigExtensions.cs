using Microsoft.Extensions.DependencyInjection;
using SBL.Infrastructure.PayPal;
using SBL.Infrastructure.Repositories;
using SBL.Services.Contracts;
using SBL.Services.Contracts.Repositories;
using SBL.Services.Contracts.Services;

namespace SBL.Infrastructure;

public static class ContainerConfigExtensions
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, OrderUnitOfWork>();
        services.AddHttpClient<IPaymentService, PayPalService>();
    }
}
