using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SBL.Domain.Entities;
using SBL.Domain.Enums;

namespace SBL.Infrastructure;

public class DataSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
    {
        var roleNames = Enum.GetNames(typeof(Role));
        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }
    }

    public static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        var defaultUsers = new[]
        {
            new User
            {
                UserName = "admin",
                Email = "admin@example.com",
                Role = Role.Admin,
                EmailConfirmed = true,
                PhoneNumber = "+380991234567",
                PhoneNumberConfirmed = true,
                SteamId = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "moderator",
                Email = "moderator@example.com",
                Role = Role.Moderator,
                EmailConfirmed = true,
                PhoneNumber = "+380997654321",
                PhoneNumberConfirmed = true,
                SteamId = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var defaultPassword = "Password123!";

        foreach (var user in defaultUsers)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(user, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, user.Role.ToString());
                }
            }
        }
    }

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
    }
}
