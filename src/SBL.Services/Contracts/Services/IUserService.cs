using SBL.Domain.Entities;
using SBL.Domain.Enums;

namespace SBL.Services.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    Task<User> GetUserAsync(int userId);

    Task DeleteUserAsync(int userId);

    Task UpdateUserAsync(User user);
    
    Task UpdateUserRoleAsync(int userId, Role role);
}
