using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Picture { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore] 
    public UserAchievement UserAchievement { get; set; }
}
