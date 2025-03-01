using SBL.Domain.Common;

namespace SBL.Services.Contracts.Services;

public interface IPaymentService
{
    Task<Result<PayPalRedirectResult>> CreatePaymentAsync(PaymentRequest request);
    
    Task<Result<PaymentExecutionResult>> ExecutePaymentAsync(string paymentId, string payerId);
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    
    public string Currency { get; set; } = "UAH";
    
    public string Description { get; set; }
    
    public string OrderId { get; set; }
    
    public string InvoiceNumber { get; set; }
}

public class PayPalRedirectResult
{
    public string PaymentId { get; set; }

    public string RedirectUrl { get; set; }
}

public class PaymentExecutionResult
{
    public string PaymentId { get; set; }
    
    public string State { get; set; }
    
    public string TransactionId { get; set; }
}
