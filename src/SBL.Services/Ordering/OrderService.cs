using Microsoft.AspNetCore.Identity;
using SBL.Domain.Contracts;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Domain.Extensions;
using SBL.Services.Contracts;
using SBL.Services.Contracts.Services;
using SBL.Services.Ordering.Models;

namespace SBL.Services.Ordering;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICouponService _couponService;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IPaymentService _paymentService;

    private readonly UserManager<User> _userManager;

    public OrderService(
        IUnitOfWork unitOfWork,
        ICouponService couponService,
        IDateTimeProvider dateTimeProvider,
        IPaymentService paymentService,
        UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _couponService = couponService;
        _dateTimeProvider = dateTimeProvider;
        _paymentService = paymentService;
        _userManager = userManager;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        var result = await _unitOfWork.OrderRepository.GetAllAsync();
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
    {
        var result = await _unitOfWork.OrderRepository.GetOrdersOfUser(userId);
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }

    public async Task<Order> GetOrderAsync(int orderId)
    {
        var result = await _unitOfWork.OrderRepository.GetOrderAsync(orderId);
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest orderRequest)
    {
        var currTime = _dateTimeProvider.Now;
        var newOrder = new Order
        {
            Status = OrderStatus.Created,
            Address = orderRequest.Address,
            Subtotal = 0,
            PaymentStatus = PaymentStatus.NotPaid,
            HasCoupon = orderRequest.HasCoupon,
            Discount = null,
            DeliveryFee = orderRequest.DeliveryFee,
            DeliveredDate = null,
            CouponId = null,
            UserId = orderRequest.UserId,
            CreatedAt = currTime,
            UpdatedAt = currTime,
            OrderItems = new List<OrderItem>(),
        };

        try
        {
            _unitOfWork.BeginTransaction();
            await CheckIfUserExists(orderRequest.UserId);
            var createdOrder = await _unitOfWork.OrderRepository.CreateAsync(newOrder);
            newOrder.Id = createdOrder.Id;
            var orderItems = await CreateOrderItemsAsync(orderRequest.OrderItems, newOrder.Id);
            newOrder.OrderItems = orderItems;
            newOrder.Subtotal = CalculateSubtotal(orderItems);
            if (orderRequest.HasCoupon && !string.IsNullOrEmpty(orderRequest.CouponCode))
            {
                var couponId = await ApplyCouponAsync(newOrder, orderRequest.CouponCode);
                newOrder.CouponId = couponId;
            }

            newOrder.Total = CalculateTotal(newOrder);
            await _unitOfWork.OrderRepository.UpdateAsync(newOrder);
            var commitResult = await _unitOfWork.CommitTransactionAsync();
            commitResult.OnFailure(() => throw new InvalidOperationException(commitResult.Error));

            return newOrder;
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();
            throw new InvalidOperationException($"Failed to place order: {ex.Message}", ex);
        }

        async Task CheckIfUserExists(int userId)
        {
            var userResult = await _userManager.FindByIdAsync(userId.ToString());
            if (userResult == null)
            {
                throw new ArgumentException();
            }
        }

        decimal CalculateSubtotal(List<OrderItem> orderItems) => orderItems.Sum(oi => oi.Total);
    }

    public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var result = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        result.OnFailure(() => throw new Exception($"Order not found: {orderId}"));
        result.Value.Status = status;
        if (status == OrderStatus.PaymentReceived)
        {
            result.Value.PaymentDate = DateTime.UtcNow;
        }
            
        await _unitOfWork.OrderRepository.UpdateAsync(result.Value);
        await _unitOfWork.CommitTransactionAsync();
        
        return result.Value;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(int orderId)
    {
        var result = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        result.OnFailure(() => throw new Exception($"Order not found: {orderId}"));
        var order = result.Value;
        if (order.Status != OrderStatus.Created)
        {
            throw new Exception($"Order is not in the correct state for payment: {order.Status}");
        }

        // Update order status to indicate payment in progress
        order.Status = OrderStatus.PendingPayment;
        _unitOfWork.BeginTransaction();
        await _unitOfWork.OrderRepository.UpdateAsync(order);

        // Create payment request to PayPal
        var paymentRequest = new PaymentRequest
        {
            Amount = CalculateTotal(order),
            Description = $"Order #{order.Id}",
            OrderId = order.Id.ToString(),
            InvoiceNumber = $"INV-{order.Id}"
        };

        try
        {
            var paymentResult = await _paymentService.CreatePaymentAsync(paymentRequest);

            if (paymentResult.Failure)
            {
                order.Status = OrderStatus.PaymentFailed;
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                await _unitOfWork.CommitTransactionAsync();

                return new PaymentResult
                {
                    Success = false,
                    OrderId = orderId,
                };
            }

            // Store the PayPal payment ID in the order
            order.PaymentId = paymentResult.Value.PaymentId;
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            return new PaymentResult
            {
                Success = true,
                OrderId = orderId,
                RedirectUrl = paymentResult.Value.RedirectUrl
            };
        }
        catch (Exception ex)
        {
            // Update order on exception
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            return new PaymentResult
            {
                Success = false,
                OrderId = orderId,
                ErrorMessage = $"Payment processing error: {ex.Message}"
            };
        }
    }

    public async Task<Order> HandlePaymentCallbackAsync(int orderId, string paymentId, string payerId,
        bool isSuccessful)
    {
        var result = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        result.OnFailure(() => throw new Exception($"Order not found: {orderId}"));
        var order = result.Value;

        if (!isSuccessful)
        {
            // Payment was cancelled or failed at PayPal
            order.Status = OrderStatus.PaymentFailed;
            
            await _unitOfWork.OrderRepository.UpdateAsync(order);
        }

        try
        {
            _unitOfWork.BeginTransaction();
            // Execute the payment with PayPal to complete it
            var executionResult = await _paymentService.ExecutePaymentAsync(paymentId, payerId);

            executionResult.OnFailure(() =>
            {
                order.Status = OrderStatus.PaymentFailed;
                _unitOfWork.OrderRepository.UpdateAsync(order);
            });

            // Update the order with payment details
            order.Status = OrderStatus.Processing;
            order.PaymentDate = DateTime.UtcNow;
            order.PaymentId = paymentId;
            order.PayerId = payerId;
            order.TransactionId = executionResult.Value.TransactionId;
            order.PaymentState = executionResult.Value.State;
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            return order;
        }
        catch (Exception ex)
        {
            order.Status = OrderStatus.PaymentFailed;
            
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            return order;
        }
    }
    
    public async Task UpdateOrderAsync(Order order)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            var result = await _unitOfWork.CommitTransactionAsync();
            result.OnFailure(() => throw new Exception(result.Error));
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();
            throw new Exception($"Failed to update order: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId)
    {
        var result = await _unitOfWork.OrderItemRepository.GetOrderItemsAsync(orderId);
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }

    private async Task<int> ApplyCouponAsync(Order order, string couponCode)
    {
        try
        {
            var coupon = await _couponService.ValidateCouponAsync(couponCode);
            order.Discount = order.Subtotal * (decimal)coupon.Percentage;

            return coupon.Id;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to apply coupon: {ex.Message}");
        }
    }

    private async Task<List<OrderItem>> CreateOrderItemsAsync(List<CreateOrderItemDto> items, int orderId)
    {
        var productIds = new int[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            productIds[i] = items[i].ProductId;
        }

        var result = await _unitOfWork.ProductRepository.GetByIdsAsync(productIds);
        result.OnFailure(() => throw new Exception(result.Error));
        var products = result.Value;
        var orderItems = new List<OrderItem>();
        var productDict = products.ToDictionary(p => p.Id);

        foreach (var item in items)
        {
            if (productDict.TryGetValue(item.ProductId, out var product))
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Total = item.Quantity * product.Price
                };

                var createdOrderItem = await _unitOfWork.OrderItemRepository.CreateAsync(orderItem);
                orderItems.Add(createdOrderItem);
            }
            else
            {
                throw new InvalidOperationException($"Product with ID {item.ProductId} not found");
            }
        }

        return orderItems;
    }

    private decimal CalculateTotal(Order order) => order.Subtotal - order.Discount ?? 0 + order.DeliveryFee;
}
