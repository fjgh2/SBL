using System.Text.Json.Serialization;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.Steam;

public class UserGameStatistic
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int GameId { get; set; }
    
    public decimal PlaytimeHours { get; set; }
    
    public int AchievementsEarned { get; set; }
    
    public int TotalAchievements { get; set; }
    
    public string StatisticsData { get; set; } // JSON data for detailed statistics
    
    public DateTime LastPlayed { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
    
    [JsonIgnore]
    public Game Game { get; set; }
}
