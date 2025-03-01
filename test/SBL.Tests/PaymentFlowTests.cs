using Microsoft.Extensions.DependencyInjection;
using Moq;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Contracts;
using SBL.Services.Contracts.Services;
using SBL.Services.Ordering;
using SBL.Services.Ordering.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SBL.Domain.Contracts;
using Xunit;

namespace SBL.Tests.Integration;

public class PaymentFlowTests
{
    private readonly Mock<IPaymentService> _mockPaymentService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICouponService> _mockCouponService;
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
    private readonly Mock<UserManager<User>> _mockUserManager;
    
    private readonly OrderService _orderService;
    
    public PaymentFlowTests()
    {
        // Setup mocks
        _mockPaymentService = new Mock<IPaymentService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCouponService = new Mock<ICouponService>();
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        
        // Setup UserManager mock (requires special setup)
        // _mockUserManager = MockUserManager<User>();
        
        // Create service under test
        _orderService = new OrderService(
            _mockUnitOfWork.Object,
            _mockCouponService.Object,
            _mockDateTimeProvider.Object,
            _mockPaymentService.Object,
            _mockUserManager.Object);
    }
    
    [Fact]
    public async Task ProcessPayment_SuccessfulPaymentCreation_ReturnsSuccessWithRedirectUrl()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { 
            Id = orderId, 
            Status = OrderStatus.Created,
            Subtotal = 100m,
            DeliveryFee = 10m
        };
        
        _mockUnitOfWork.Setup(u => u.OrderRepository.GetByIdAsync(orderId))
            .ReturnsAsync(Result<Order>.Success(order));
            
        // _mockPaymentService.Setup(p => p.CreatePaymentAsync(It.IsAny<PaymentRequest>()))
        //     .ReturnsAsync(Result<PaymentCreationResult>.Success(new PaymentCreationResult 
        //     { 
        //         PaymentId = "PAY-123", 
        //         RedirectUrl = "https://paypal.com/checkout" 
        //     }));
        
        // Act
        var result = await _orderService.ProcessPaymentAsync(orderId);
        
        // Assert
        Assert.True(result.Success);
        Assert.Equal("https://paypal.com/checkout", result.RedirectUrl);
        Assert.Equal(orderId, result.OrderId);
    }
    
    [Fact]
    public async Task HandlePaymentCallback_SuccessfulPayment_UpdatesOrderCorrectly()
    {
        // Arrange
        var orderId = 1;
        var paymentId = "PAY-123";
        var payerId = "PAYER-456";
        
        var order = new Order { 
            Id = orderId, 
            Status = OrderStatus.PendingPayment,
            PaymentId = paymentId
        };
        
        _mockUnitOfWork.Setup(u => u.OrderRepository.GetByIdAsync(orderId))
            .ReturnsAsync(Result<Order>.Success(order));
            
        _mockPaymentService.Setup(p => p.ExecutePaymentAsync(paymentId, payerId))
            .ReturnsAsync(Result<PaymentExecutionResult>.Success(new PaymentExecutionResult 
            { 
                TransactionId = "TRANS-789", 
                State = "approved" 
            }));
        
        // Act
        var result = await _orderService.HandlePaymentCallbackAsync(orderId, paymentId, payerId, true);
        
        // Assert
        Assert.Equal(OrderStatus.Processing, result.Status);
        Assert.Equal(PaymentStatus.Paid, result.PaymentStatus);
        Assert.Equal("TRANS-789", result.TransactionId);
        Assert.Equal("approved", result.PaymentState);
    }
}
