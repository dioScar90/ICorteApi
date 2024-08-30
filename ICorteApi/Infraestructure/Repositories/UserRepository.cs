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
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _context;
    private readonly DbSet<User> _dbSet;
    private readonly IUserErrors _errors;

    private readonly int? _userId;
    private readonly User? _user;

    public UserRepository(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        AppDbContext context,
        IUserErrors errors)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _dbSet = _context.Set<User>();

        if (httpContextAccessor.HttpContext?.User is ClaimsPrincipal user)
        {
            _userId = int.TryParse(_userManager.GetUserId(user), out int userId) ? userId : default;
            _user = userManager.GetUserAsync(user).GetAwaiter().GetResult();
        }

        _errors = errors;
    }

    private async Task RegenerateUserCookieAsync(User user) => await _signInManager.RefreshSignInAsync(user);
    private async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    private static async Task CommitAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();
    private static async Task RollbackAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();

    public async Task<User?> CreateUserAsync(User newUser, string password)
    {
        var transaction = await BeginTransactionAsync();

        try
        {
            var userResult = await _userManager.CreateAsync(newUser, password);

            if (!userResult.Succeeded)
                _errors.ThrowCreateException([..userResult.Errors]);

            var roleResult = await _userManager.AddToRoleAsync(newUser, nameof(UserRole.Guest));

            if (!roleResult.Succeeded)
                _errors.ThrowBasicUserException([..userResult.Errors]);

            await RegenerateUserCookieAsync(newUser);

            await CommitAsync(transaction);
            return await GetMeAsync(true);
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public int? GetMyUserId() => _userId;

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        if (_user is null)
            return [];

        var roles = await _userManager.GetRolesAsync(_user);

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

    private async void UpdatedUserEntityNow(User user)
    {
        user.UpdatedUserNow();
        await _userManager.UpdateAsync(user);
    }

    public async Task<bool> AddUserRoleAsync(UserRole role)
    {
        var res = await _userManager.AddToRoleAsync(_user!, Enum.GetName(role)!);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return false;
        }

        UpdatedUserEntityNow(_user!);

        await RegenerateUserCookieAsync(_user!);
        return true;
    }

    public async Task<bool> RemoveFromRoleAsync(UserRole role)
    {
        var res = await _userManager.RemoveFromRoleAsync(_user!, Enum.GetName(role)!);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return false;
        }

        UpdatedUserEntityNow(_user!);

        await RegenerateUserCookieAsync(_user!);
        return true;
    }

    public async Task<bool> UpdateEmailAsync(string newEmail)
    {
        var identityResult = await _userManager.SetEmailAsync(_user!, newEmail);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdateEmailException([.. identityResult.Errors]);

        UpdatedUserEntityNow(_user!);
        return true;
    }

    public async Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        var identityResult = await _userManager.ChangePasswordAsync(_user!, currentPassword, newPassword);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdatePasswordException([.. identityResult.Errors]);

        UpdatedUserEntityNow(_user!);
        return true;
    }

    public async Task<bool> UpdatePhoneNumberAsync(string newPhoneNumber)
    {
        var identityResult = await _userManager.SetPhoneNumberAsync(_user!, newPhoneNumber);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdatePhoneNumberException([.. identityResult.Errors]);

        UpdatedUserEntityNow(_user!);
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
            var res = await _userManager.DeleteAsync(user);

            if (!res.Succeeded)
                _errors.ThrowBasicUserException([..res.Errors]);

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
