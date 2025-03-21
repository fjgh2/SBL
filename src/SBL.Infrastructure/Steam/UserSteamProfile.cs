using System.Text.Json.Serialization;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.Steam;

public class UserSteamProfile
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    
    public string ProfileUrl { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public string SteamUsername { get; set; }
    
    public DateTime LastSyncDate { get; set; }
    
    public string SteamProfileData { get; set; } // JSON data from Steam API
    
    [JsonIgnore]
    public User User { get; set; }
    
    [JsonIgnore]
    public ICollection<UserGameStatistic> GameStatistics { get; set; } = new List<UserGameStatistic>();
}
