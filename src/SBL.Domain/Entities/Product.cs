using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }

    public string Picture { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    // [JsonIgnore]
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    [JsonIgnore]
    public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    
    [JsonIgnore]
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // [JsonIgnore] 
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
