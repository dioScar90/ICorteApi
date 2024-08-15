using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Settings;

public class RoleSeeder
{
    // public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
    // {
    //     foreach (var role in Enum.GetNames(typeof(UserRole)))
    //     {
    //         bool roleExists = await roleManager.RoleExistsAsync(role);

    //         if (!roleExists)
    //             await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
    //     }
    // }
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // foreach (var role in UserRole.GetAllRoles())
        foreach (var role in Enum.GetNames(typeof(UserRole)))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}
