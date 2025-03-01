namespace SBL.Domain.Entities;

public class News
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Text { get; set; }

    // pictures if needed
    // if used in achievements, make it an entity
    public int Vote { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}
