namespace SBL.Services.Ordering.Models;

public class CreateOrderRequest
{
    public string Address { get; set; }
    
    public int UserId { get; set; }
    
    public bool HasCoupon { get; set; }
    
    public string CouponCode { get; set; }
    
    public decimal DeliveryFee { get; set; }
    
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
}
