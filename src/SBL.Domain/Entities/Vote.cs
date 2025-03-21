using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class Vote
{
    public int Id { get; set; }
    
    public int Count { get; set; }
    
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
}
