using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class UserRepository(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    : IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<User> _userManager = userManager;

    private async Task<User?> GetCurrentUser()
    {
        var userId = GetUserId();
        
        if (userId is null)
            return null;

        return await _userManager.FindByIdAsync(userId);
    }
    
    public string? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            return null;

        return _userManager.GetUserId(user);
    }

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        var user = await GetCurrentUser();
        
        if (user is null)
            return [];

        var roles = await _userManager.GetRolesAsync(user);

        if (roles is null)
            return [];

        return roles
            .Select(role => Enum.TryParse<UserRole>(role, out var userRole) ? userRole : (UserRole?)null)
            .Where(role => role.HasValue)
            .Select(role => role!.Value)
            .ToArray();
    }

    public async Task<ISingleResponse<User>> GetMeAsync()
    {
        var userEntity = await GetCurrentUser();

        if (userEntity is null)
            return Response.Failure<User>(Error.UserNotFound);

        return Response.Success(userEntity);
    }

    public async Task<IResponse> AddUserRoleAsync(UserRole role)
    {
        var user = await GetCurrentUser();

        if (user is null)
            return Response.Failure(Error.Unauthorized);
        
        var res = await _userManager.AddToRoleAsync(user, role.ToString());

        if (!res.Succeeded)
            return Response.Failure(new Error(res.Errors.First().Code, res.Errors.First().Description));
        
        return Response.Success();
    }

    public async Task<IResponse> RemoveUserRoleAsync(UserRole role)
    {
        var user = await GetCurrentUser();

        if (user is null)
            return Response.Failure(Error.Unauthorized);
        
        var res = await _userManager.RemoveFromRoleAsync(user, role.ToString());

        if (!res.Succeeded)
            return Response.Failure(new Error(res.Errors.First().Code, res.Errors.First().Description));
        
        return Response.Success();
    }

    public async Task<IResponse> UpdateAsync(User user)
    {
        var res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded)
            return Response.Failure(new Error(res.Errors.First().Code, res.Errors.First().Description));
        
        return Response.Success();
    }
    
    public async Task<IResponse> DeleteAsync(User user)
    {
        var res = await _userManager.DeleteAsync(user);

        if (!res.Succeeded)
            return Response.Failure(new Error(res.Errors.First().Code, res.Errors.First().Description));
        
        return Response.Success();
    }
}
