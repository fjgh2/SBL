using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        await SeedRolesAsync(roleManager);

        throw new NotImplementedException();
    }
}
