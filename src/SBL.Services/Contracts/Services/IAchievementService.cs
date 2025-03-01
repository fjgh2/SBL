using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Services;

public interface IAchievementService
{
    Task<int> CreateAchievementAsync(Achievement achievement);

    Task GetUserAchievementAsync(int userId);
}
