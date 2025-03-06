using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBL.Api.Dtos;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<ActionResult<List<Order>>> GetAllOrdersAsync()
    {
        var orders = await _orderService.GetAllOrdersAsync();

        return Ok(orders);
    }

    // [Authorize(Roles = "Admin, User")]
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<List<Order>>> GetUserOrdersAsync(int userId)
    {
        if (userId < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid user id." });
        }

        var orders = await _orderService.GetUserOrdersAsync(userId);

        return Ok(orders);
    }

    // [Authorize(Roles = "Admin, User")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetOrderAsync(int id)
    {
        if (id < 1)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid order id." });
        }

        var order = await _orderService.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    // [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<Order>> PlaceOrderAsync(SBL.Services.Ordering.Models.CreateOrderRequest orderRequest)
    {
        // if (orderRequest == null)
        // {
        //     return BadRequest(new ProblemDetails() { Title = "Invalid order data" });
        // }
        var order = await _orderService.CreateOrderAsync(orderRequest);

        return Ok(order);
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult> UpdateOrder(Order order)
    {
        await _orderService.UpdateOrderAsync(order);

        return Ok();
    }

    // [Authorize(Roles = "Admin, User")]
    [HttpGet("orderItems/{orderId:int}")]
    public async Task<ActionResult<List<OrderItem>>> GetOrderItems(int orderId)
    {
        if (orderId < 1)
        {
            return BadRequest(new ProblemDetails() { Title = $"Invalid orderId: {orderId}" });
        }

        var orderItems = await _orderService.GetOrderItemsAsync(orderId);

        return Ok(orderItems);
    }

    [HttpPost("{orderId}/payment")]
    public async Task<IActionResult> ProcessPayment(string orderId)
    {
        var parsedSuccessfully = int.TryParse(orderId, out var parsedId);
        if (parsedSuccessfully)
        {
            var result = await _orderService.ProcessPaymentAsync(parsedId);

            if (!result.Success)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        return BadRequest("Invalid orderId");
    }

    [HttpGet("payment/callback")]
    public async Task<IActionResult> PaymentCallback([FromQuery] string orderId, [FromQuery] string paymentId,
        [FromQuery] string PayerID)
    {
        bool isSuccessful = !string.IsNullOrEmpty(paymentId) && !string.IsNullOrEmpty(PayerID);

        try
        {
            var parsedSuccessfully = int.TryParse(orderId, out var parsedId);
            if (parsedSuccessfully)
            {
                // Update the order based on the payment result
                var order = await _orderService.HandlePaymentCallbackAsync(parsedId, paymentId, PayerID, isSuccessful);

                // Redirect to appropriate page based on payment result
                if (order.Status == OrderStatus.PaymentReceived || order.Status == OrderStatus.Processing)
                {
                    return Redirect($"/orders/thank-you?orderId={orderId}");
                }
                else
                {
                    return Redirect($"/orders/payment-failed?orderId={orderId}");
                }
            }

            return Redirect($"/orders/payment-failed?orderId={orderId}");
        }
        catch (Exception ex)
        {
            return Redirect($"/orders/payment-failed?orderId={orderId}&error=processing_error");
        }
    }
}
