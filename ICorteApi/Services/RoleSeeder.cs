using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICorteApi.Enums;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Services;

public class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
    {
        var roles = Enum.GetNames(typeof(UserRole));
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                // await roleManager.CreateAsync(new IdentityRole(role));
                await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            }
        }
    }
}
