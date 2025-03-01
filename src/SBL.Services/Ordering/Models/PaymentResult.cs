namespace SBL.Services.Ordering.Models;

public class PaymentResult
{
    public bool Success { get; set; }
    public int OrderId { get; set; }
    public string RedirectUrl { get; set; }
    public string ErrorMessage { get; set; }
}