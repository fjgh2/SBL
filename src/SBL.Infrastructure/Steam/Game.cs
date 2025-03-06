using System.Text.Json.Serialization;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.Steam;

public class Game
{
    public int Id { get; set; }
    
    // Steam-specific properties
    public string SteamAppId { get; set; }  // Steam's unique identifier for the game
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    // Media from Steam
    public string HeaderImageUrl { get; set; }
    
    public string BackgroundImageUrl { get; set; }
    
    public string CapsuleImageUrl { get; set; }
    
    // Game details
    public string Developer { get; set; }
    
    public string Publisher { get; set; }
    
    public DateTime? ReleaseDate { get; set; }
    
    // Pricing information
    public decimal? Price { get; set; }
    
    public decimal? DiscountedPrice { get; set; }
    
    public int? DiscountPercent { get; set; }
    
    // Metadata
    public string[] Categories { get; set; }  // E.g., "Multiplayer", "Single-player"
    
    public string[] Genres { get; set; }
    
    public string[] Tags { get; set; }
    
    // Platform availability
    public bool IsWindows { get; set; }
    
    public bool IsMac { get; set; }
    
    public bool IsLinux { get; set; }
    
    // Game metrics
    public int? MetacriticScore { get; set; }
    
    public string AgeRating { get; set; }
    
    // Cache of Steam data
    public string SteamDataJson { get; set; }  // Full JSON response from Steam API
    
    // System fields
    public DateTime LastSyncedAt { get; set; }  // When data was last fetched from Steam
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public ICollection<UserGameStatistic> UserGameStatistics { get; set; } = new List<UserGameStatistic>();
}
