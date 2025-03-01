using Microsoft.AspNetCore.Mvc;
using SBL.Api.Dtos;
using SBL.Domain.Extensions;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _payPalService;

    public PaymentController(IPaymentService payPalService)
    {
        _payPalService = payPalService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
    {
        if (request == null || request.Amount <= 0)
        {
            return BadRequest("Invalid payment request");
        }

        var result = await _payPalService.CreatePaymentAsync(request);
        if (!result.Failure)
        {
            return Ok(result);
        }

        return StatusCode(500, result.Error);
    }

    [HttpGet("execute")]
    public async Task<IActionResult> ExecutePayment([FromQuery] string paymentId, [FromQuery] string PayerID,
        [FromQuery] string orderId)
    {
        if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(PayerID))
        {
            return BadRequest("Missing required parameters");
        }

        try 
        {
            var result = await _payPalService.ExecutePaymentAsync(paymentId, PayerID);
            if (!result.Failure)
            {
                if (result.Value.State.ToLower() != "approved")
                {
                    return Redirect($"/payment/failed?orderId={orderId}&reason=not_approved");
                }

                return Redirect($"/payment/success?orderId={orderId}&transactionId={result.Value.TransactionId}");
            }

            return Redirect($"/payment/failed?orderId={orderId}&reason=execution_failed");
        }
        catch (Exception ex)
        {
            return Redirect($"/payment/failed?orderId={orderId}&reason=system_error");
        }
    }
}