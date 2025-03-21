namespace SBL.Domain.Entities;

public class News
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Text { get; set; }
    
    public string Picture { get; set; }
    
    // int just to set up db
    public int Vote { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}
