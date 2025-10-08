using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Entities;

public class ApplicationRole : IdentityRole<int>
{
}

public enum UserRole
{
    Guest,
    Client,
    BarberShop,
    Admin,
}

public enum PolicyUserRole
{
    FreeIfAuthenticated,
    ClientOrHigh,
    ClientOnly,
    BarberShopOrHigh,
    AdminOnly,
}

public static partial class PolicyUserRoleExtensions
{
    public static string[] GetRolesString(this PolicyUserRole policy) =>
        [.. policy.GetRolesEnum().Select(role => role.ToString())];

    public static UserRole[] GetRolesEnum(this PolicyUserRole policy) => policy switch
    {
        PolicyUserRole.AdminOnly =>
            [
                UserRole.Admin,
            ],

        PolicyUserRole.BarberShopOrHigh =>
            [
                UserRole.BarberShop,
                UserRole.Admin,
            ],

        PolicyUserRole.ClientOnly =>
            [
                UserRole.Client,
                UserRole.Admin,
            ],

        PolicyUserRole.ClientOrHigh =>
            [
                UserRole.Client,
                UserRole.BarberShop,
                UserRole.Admin,
            ],

        PolicyUserRole.FreeIfAuthenticated =>
            [
                UserRole.Guest,
                UserRole.Client,
                UserRole.BarberShop,
                UserRole.Admin,
            ],

        _ => []
    };
}
