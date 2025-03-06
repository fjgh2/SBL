using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class NewsComment
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public int UserId { get; set; }
    
    public int NewsId { get; set; }
    
    public int? ParentCommentId { get; set; } // For replies to comments
    
    public bool IsModeratorResponse { get; set; } // Flag for moderator responses
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
    
    [JsonIgnore]
    public News News { get; set; }
    
    [JsonIgnore]
    public NewsComment ParentComment { get; set; }
    
    [JsonIgnore]
    public ICollection<NewsComment> Replies { get; set; } = new List<NewsComment>();
}
