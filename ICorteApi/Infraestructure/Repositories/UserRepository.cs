using System.Collections.ObjectModel;
using System.Security.Claims;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IHttpContextAccessor _httpCtx;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _context;
    private readonly DbSet<User> _dbSet;
    private readonly IUserErrors _errors;

    private int? UserId;
    private User? User;

    public UserRepository(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        AppDbContext context,
        IUserErrors errors)
    {
        _httpCtx = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _dbSet = _context.Set<User>();

        _errors = errors;

        _ = SetInitUser();

        // if (_httpCtx.HttpContext?.User is ClaimsPrincipal user)
        // {
        //     UserId = int.TryParse(_userManager.GetUserId(user), out int userId) ? userId : default;
        //     User = userManager.GetUserAsync(user).GetAwaiter().GetResult();
        // }
    }

    private async Task SetInitUser()
    {
        if (_httpCtx.HttpContext?.User is not ClaimsPrincipal user)
            return;

        if (int.TryParse(_userManager.GetUserId(user), out int userId))
            return;

        UserId ??= userId;
        User ??= await _userManager.GetUserAsync(user);
    }

    private async Task RegenerateUserCookieAsync(User? user = null) => await _signInManager.RefreshSignInAsync(user ?? User!);
    private async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    private static async Task CommitAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();
    private static async Task RollbackAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();

    public async Task<User?> CreateUserAsync(User newUser, string password)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            var userIdentityResult = await _userManager.CreateAsync(newUser, password);

            if (!userIdentityResult.Succeeded)
                _errors.ThrowCreateException([.. userIdentityResult.Errors]);

            // User = await userManager.GetUserAsync(user);
            // await SetInitUser();
            var roleIdentityResult = await _userManager.AddToRoleAsync(newUser, nameof(UserRole.Guest));

            if (!roleIdentityResult.Succeeded)
                _errors.ThrowBasicUserException([.. roleIdentityResult.Errors]);

            // await RegenerateUserCookieAsync(newUser);
            await CommitAsync(transaction);
            return newUser;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }
    
    private async Task<User?> GetMyUserEntityAsync() =>
        _httpCtx.HttpContext?.User is null ? null : await _userManager.GetUserAsync(_httpCtx.HttpContext.User);
    
    public async Task<int?> GetMyUserIdAsync() => (await GetMyUserEntityAsync())?.Id;

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        if (await GetMyUserEntityAsync() is not User user)
            return [];
            
        var userRoles = (await _userManager.GetRolesAsync(user))
            .Aggregate(
                new HashSet<UserRole>(),
                (roles, role) => !Enum.TryParse<UserRole>(role, out var userRole) ? [..roles] : [..roles, userRole],
                item => item.ToArray()
            );

        return userRoles ?? [];
    }

    public async Task<User?> GetMeAsync(bool? dispatchIncludes = false)
    {
        if (dispatchIncludes == true)
            return await GetMyUserEntityAsync();

        if (await GetMyUserIdAsync() is not int userId)
            return null;

        var user = await _dbSet
            .Include(u => u.Profile)
            .Include(u => u.BarberShop)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return null;

        user.SetRoles(await GetUserRolesAsync());
        return user;
    }

    private async Task UpdatedUserEntityNow()
    {
        User!.UpdatedUserNow();
        await _userManager.UpdateAsync(User);
    }

    public async Task<bool> AddUserRoleAsync(UserRole role)
    {
        var identityResult = await _userManager.AddToRoleAsync(User!, Enum.GetName(role)!);

        if (!identityResult.Succeeded)
            _errors.ThrowAddUserRoleException([.. identityResult.Errors]);

        await UpdatedUserEntityNow();
        await RegenerateUserCookieAsync();

        return true;
    }

    public async Task<bool> RemoveFromRoleAsync(UserRole role)
    {
        var identityResult = await _userManager.RemoveFromRoleAsync(User!, Enum.GetName(role)!);

        if (!identityResult.Succeeded)
            _errors.ThrowRemoveUserRoleException([.. identityResult.Errors]);

        await UpdatedUserEntityNow();
        await RegenerateUserCookieAsync();

        return true;
    }

    public async Task<bool> UpdateEmailAsync(string newEmail)
    {
        var identityResult = await _userManager.SetEmailAsync(User!, newEmail);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdateEmailException([.. identityResult.Errors]);

        await UpdatedUserEntityNow();
        return true;
    }

    public async Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        var identityResult = await _userManager.ChangePasswordAsync(User!, currentPassword, newPassword);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdatePasswordException([.. identityResult.Errors]);

        await UpdatedUserEntityNow();
        return true;
    }

    public async Task<bool> UpdatePhoneNumberAsync(string newPhoneNumber)
    {
        var identityResult = await _userManager.SetPhoneNumberAsync(User!, newPhoneNumber);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdatePhoneNumberException([.. identityResult.Errors]);

        await UpdatedUserEntityNow();
        return true;
    }

    private async void DeleteUserEntity(User user)
    {
        user.DeleteEntity();
        await _userManager.UpdateAsync(user);
    }

    public async Task<bool> DeleteAsync(User user)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            string[] roles = Enum.GetNames(typeof(UserRole));
            var roleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!roleResult.Succeeded)
                _errors.ThrowBasicUserException([.. roleResult.Errors]);

            DeleteUserEntity(user);
            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
                _errors.ThrowBasicUserException([.. identityResult.Errors]);

            await CommitAsync(transaction);
            return true;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }
}
