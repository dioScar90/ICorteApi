using System.Security.Claims;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Extensions;

public static class AuthorizationExtensions
{
    public static bool IsAuthorized(this ClaimsPrincipal user, params UserRole[] requiredRoles)
    {
        if (user.Identity is { IsAuthenticated: true })
        {
            var roleClaims = user.FindAll(ClaimTypes.Role).Select(c => Enum.Parse<UserRole>(c.Value));
            return requiredRoles.Any(role => roleClaims.Contains(role));
        }
        return false;
    }
}
