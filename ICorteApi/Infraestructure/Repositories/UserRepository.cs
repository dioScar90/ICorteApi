using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class UserRepository(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, AppDbContext context)
    : IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<User> _userManager = userManager;
    private readonly AppDbContext _context = context;
    private readonly DbSet<User> _dbSet = context.Set<User>();
    
    public async Task<ISingleResponse<User>> CreateUserAsync(User newUser, string password)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var userResult = await _userManager.CreateAsync(newUser, password);

            if (!userResult.Succeeded)
                throw new Exception(userResult.Errors.First().Description);
                
            var roleResult = await _userManager.AddToRoleAsync(newUser, nameof(UserRole.Guest));

            if (!roleResult.Succeeded)
                throw new Exception(roleResult.Errors.First().Description);

            await transaction.CommitAsync();
            return Response.Success(newUser);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<User>(new("TransactionError", ex.Message));
        }
    }
    
    private string? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            return null;

        return _userManager.GetUserId(user);
    }

    public int? GetMyUserId() => int.TryParse(GetUserId(), out int userId) ? userId : null;

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

    private async Task<User?> GetCurrentUser()
    {
        var userId = GetUserId();
        
        if (userId is null)
            return null;

        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ISingleResponse<User>> GetMeAsync()
    {
        var userId = GetMyUserId();

        if (userId is not int id)
            return Response.Failure<User>(Error.UserNotFound);

        var userEntity = await _dbSet
            .Include(u => u.Person)
            .Include(u => u.OwnedBarberShop)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (userEntity is not User user)
            return Response.Failure<User>(Error.UserNotFound);

        return Response.Success(user);
    }

    private async void UpdatedUserEntityNow(User user)
    {
        user.UpdatedUserNow();
        await _userManager.UpdateAsync(user);
    }

    public async Task<IResponse> AddUserRoleAsync(UserRole role)
    {
        var user = (await GetCurrentUser())!;
        var res = await _userManager.AddToRoleAsync(user, Enum.GetName(role)!);
        
        if (!res.Succeeded)
            return Response.Failure(new(res.Errors.First().Code, res.Errors.First().Description));
        
        UpdatedUserEntityNow(user);
        return Response.Success();
    }

    public async Task<IResponse> RemoveFromRoleAsync(UserRole role)
    {
        var user = (await GetCurrentUser())!;
        var res = await _userManager.RemoveFromRoleAsync(user, Enum.GetName(role)!);

        if (!res.Succeeded)
            return Response.Failure(new(res.Errors.First().Code, res.Errors.First().Description));
        
        UpdatedUserEntityNow(user);
        return Response.Success();
    }
    
    public async Task<IResponse> UpdateEmailAsync(string newEmail)
    {
        var user = (await GetCurrentUser())!;
        var res = await _userManager.SetEmailAsync(user, newEmail);

        if (!res.Succeeded)
            return Response.Failure(new(res.Errors.First().Code, res.Errors.First().Description));
        
        UpdatedUserEntityNow(user);
        return Response.Success();
    }
    
    public async Task<IResponse> UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        var user = (await GetCurrentUser())!;
        var res = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!res.Succeeded)
            return Response.Failure(new(res.Errors.First().Code, res.Errors.First().Description));
        
        UpdatedUserEntityNow(user);
        return Response.Success();
    }

    public async Task<IResponse> UpdatePhoneNumberAsync(string newPhoneNumber)
    {
        var user = (await GetCurrentUser())!;
        var res = await _userManager.SetPhoneNumberAsync(user, newPhoneNumber);

        if (!res.Succeeded)
            return Response.Failure(new(res.Errors.First().Code, res.Errors.First().Description));
        
        UpdatedUserEntityNow(user);
        return Response.Success();
    }

    private async void DeleteUserEntity(User user)
    {
        user.DeleteEntity();
        await _userManager.UpdateAsync(user);
    }
    
    public async Task<IResponse> DeleteAsync(User user)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Remove from all rules.
            string[] roles = Enum.GetNames(typeof(UserRole));
            var roleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!roleResult.Succeeded)
                throw new Exception(roleResult.Errors.First().Description);
            
            DeleteUserEntity(user);
            var res = await _userManager.DeleteAsync(user);

            if (!res.Succeeded)
                throw new Exception(res.Errors.First().Description);
            
            await transaction.CommitAsync();
            return Response.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<User>(new("TransactionError", ex.Message));
        }
    }
}
