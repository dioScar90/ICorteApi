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
    private readonly IHttpContextAccessor _httpContextAccessor;
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
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _dbSet = _context.Set<User>();

        _errors = errors;

        _ = SetInitUser();

        // if (_httpContextAccessor.HttpContext?.User is ClaimsPrincipal user)
        // {
        //     UserId = int.TryParse(_userManager.GetUserId(user), out int userId) ? userId : default;
        //     User = userManager.GetUserAsync(user).GetAwaiter().GetResult();
        // }
    }

    private async Task SetInitUser()
    {
        if (_httpContextAccessor.HttpContext?.User is not ClaimsPrincipal user)
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
        var transaction = await BeginTransactionAsync();

        try
        {
            var userIdentityResult = await _userManager.CreateAsync(newUser, password);

            if (!userIdentityResult.Succeeded)
                _errors.ThrowCreateException([..userIdentityResult.Errors]);

            // User = await userManager.GetUserAsync(user);
            // await SetInitUser();
            var roleIdentityResult = await _userManager.AddToRoleAsync(newUser, nameof(UserRole.Guest));

            if (!roleIdentityResult.Succeeded)
                _errors.ThrowBasicUserException([..roleIdentityResult.Errors]);

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

    public int? GetMyUserId() => UserId;

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        if (User is null)
            return [];

        var roles = await _userManager.GetRolesAsync(User);

        if (roles is null)
            return [];

        var userRoles = roles
            .Select(role => Enum.TryParse<UserRole>(role, out var userRole) ? userRole : (UserRole?)null)
            .Where(role => role.HasValue)
            .Select(role => role!.Value)
            .ToArray();

        return userRoles ?? [];
    }

    public async Task<User?> GetMeAsync(bool? dispatchIncludes = false)
    {
        var userId = GetMyUserId();

        if (userId is not int id)
            return null;
        
        if (dispatchIncludes == true)
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

        var userEntity = await _dbSet
            .Include(u => u.Profile)
            .Include(u => u.OwnedBarberShop)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (userEntity is not User user)
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
                _errors.ThrowBasicUserException([..roleResult.Errors]);

            DeleteUserEntity(user);
            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
                _errors.ThrowBasicUserException([..identityResult.Errors]);

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
