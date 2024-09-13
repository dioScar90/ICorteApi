using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Entities;

public static class UserRoles
{
    public const string Guest = "Guest";
    public const string Client = "Client";
    public const string BarberShop = "BarberShop";
    public const string Admin = "Admin";

    public static List<string> GetAllRoles() => [Guest, Client, BarberShop, Admin];
}

public class ApplicationRole : IdentityRole<int>
{
}

public enum PolicyUserRole
{
    FreeIfAuthenticated,
    ClientOrHigh,
    ClientOnly,
    BarberShopOrHigh,
    AdminOnly
}

public enum UserRole
{
    Guest,
    Client,
    BarberShop,
    Admin,
}
