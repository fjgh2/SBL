using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }
    
    public string Code { get; set; }
    
    public double Percentage { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int UsedCount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
