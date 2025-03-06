using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class NewsTag
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [JsonIgnore]
    public ICollection<News> News { get; set; } = new List<News>();
}
