using System.Text.Json.Serialization;
using SBL.Domain.Enums;

namespace SBL.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public string Address { get; set; }
    
    public decimal Total { get; set; }

    public decimal Subtotal { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
    
    public string PaymentId { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public string PayerId { get; set; }
    
    public string TransactionId { get; set; }
    
    public string PaymentState { get; set; }
    
    public bool HasCoupon { get; set; }
    
    public decimal? Discount { get; set; }
    
    public decimal DeliveryFee { get; set; }
    
    public DateTime? DeliveredDate { get; set; }
    
    public int? CouponId { get; set; }

    public int? UserId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public Coupon Coupon { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
    
    [JsonIgnore]
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
