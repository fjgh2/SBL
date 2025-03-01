using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Contracts.Services;

namespace SBL.Services.UserManagement;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    
    private RoleManager<IdentityRole<int>> _roleManager;

    public UserService(UserManager<User> userManager, 
        RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task UpdateUserAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception($"Update failed: {result.Errors.Select(e => e.Description)}");
        }
    }

    public async Task UpdateUserRoleAsync(int userId, Role role)
    {
        var user = await GetUserAsync(userId);

        if (user == null)
        {
            throw new ArgumentException("No such user");
        }
        var currentRoles = await _userManager.GetRolesAsync(user);
        
        // Remove current roles
        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                throw new Exception($"Failed to remove existing roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
            }
        }
        // Update user's Role property
        user.Role = role;
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            throw new Exception($"Update failed: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
        }

        // Ensure the new role exists
        var newRoleName = role.ToString();
        if (!await _roleManager.RoleExistsAsync(newRoleName))
        {
            throw new ArgumentException();
        }

        // Add the new role
        var addResult = await _userManager.AddToRoleAsync(user, newRoleName);
        if (!addResult.Succeeded)
        {
            throw new Exception($"Failed to add new role: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
        }
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Update failed: {result.Errors.Select(e => e.Description)}");
            }
        }
    }
}
