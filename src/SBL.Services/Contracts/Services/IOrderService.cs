using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Ordering;
using SBL.Services.Ordering.Models;

namespace SBL.Services.Contracts.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest order);

    Task<Order> HandlePaymentCallbackAsync(int orderId, string paymentId, string payerId, bool isSuccessful);

    Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);

    Task<PaymentResult> ProcessPaymentAsync(int orderId);
    
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    
    Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
    
    Task<Order> GetOrderAsync(int orderId);
    
    Task UpdateOrderAsync(Order order);
    
    Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
}
