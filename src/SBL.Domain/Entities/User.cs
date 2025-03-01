using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using SBL.Domain.Enums;

namespace SBL.Domain.Entities;

public class User : IdentityUser<int>
{
    public Role Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    
    [JsonIgnore]
    public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    [JsonIgnore] 
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
