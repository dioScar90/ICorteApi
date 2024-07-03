using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Application.Services;

public class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
    {
        foreach (var role in Enum.GetNames(typeof(UserRole)))
        {
            bool roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
                await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
        }
    }
}
