namespace SBL.Domain.Entities;

public class UserAchievement
{
    public int UserId { get; set; }
    
    public int AchievementId { get; set; }

    public DateTime CreatedAt { get; set; }
}
