using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpCtx;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _context;
    private readonly DbSet<User> _dbSet;
    private readonly IUserErrors _errors;
    
    public UserService(
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
    }
    
    private async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    private static async Task CommitAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();
    private static async Task RollbackAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();
    
    private async Task<User?> GetMyUserEntityAsync() =>
        _httpCtx.HttpContext?.User is null ? null : await _userManager.GetUserAsync(_httpCtx.HttpContext.User);
    
    private async Task RegenerateUserCookieAsync(User? user = null) =>
        await _signInManager.RefreshSignInAsync(user ?? await GetMyUserEntityAsync());
        
    public async Task<int> GetMyUserIdAsync()
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            _errors.ThrowDeuRuimException();

        return user!.Id;
    }

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        if (await GetMyUserEntityAsync() is not User user)
            return [];

        var userRoles = (await _userManager.GetRolesAsync(user))
            .Aggregate(
                new HashSet<UserRole>(),
                (roles, role) => !Enum.TryParse<UserRole>(role, out var userRole) ? [.. roles] : [.. roles, userRole],
                item => item.ToArray()
            );

        return userRoles ?? [];
    }

    private static HashSet<string> GetUserRolesToBeSetted(User user)
    {
        HashSet<string> roles = [nameof(UserRole.Guest)];

        if (user.Profile is not null)
        {
            roles.Add(nameof(UserRole.Client));
            
            if (user.BarberShop is not null)
                roles.Add(nameof(UserRole.BarberShop));
        }
        
        return roles;
    }
    
    public async Task<User?> CreateAsync(UserDtoRegisterCreate dto)
    {
        var newUser = new User(dto);
        
        using var transaction = await BeginTransactionAsync();

        try
        {
            var userIdentityResult = await _userManager.CreateAsync(newUser, newUser.GetPasswordToBeHashed());

            if (!userIdentityResult.Succeeded)
                _errors.ThrowCreateException([..userIdentityResult.Errors]);
                
            var roleIdentityResult = await _userManager.AddToRolesAsync(newUser, [..GetUserRolesToBeSetted(newUser)]);

            if (!roleIdentityResult.Succeeded)
                _errors.ThrowBasicUserException([..roleIdentityResult.Errors]);
            
            await CommitAsync(transaction);
            return newUser;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public async Task<User?> GetMeAsync(bool? dispatchIncludes = false)
    {
        if (dispatchIncludes == true)
            return await GetMyUserEntityAsync();

        int userId = await GetMyUserIdAsync();

        var user = await _dbSet
            .AsNoTracking()
            .Include(u => u.Profile)
            .Include(u => u.BarberShop)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return null;

        user.SetRoles(await GetUserRolesAsync());
        return user;
    }

    public async Task<User> GetMyUserAsync() => (await GetMeAsync())!;

    private async Task UpdatedUserEntityNow(User user)
    {
        user.UpdatedUserNow();
        await _userManager.UpdateAsync(user);
    }
    
    public async Task<bool> AddUserRoleAsync(UserRole role)
    {
        var user = await GetMyUserEntityAsync();
        var identityResult = await _userManager.AddToRoleAsync(user, role.ToString());

        if (!identityResult.Succeeded)
            _errors.ThrowAddUserRoleException([..identityResult.Errors]);

        await UpdatedUserEntityNow(user);
        await RegenerateUserCookieAsync();

        return true;
    }
    
    public async Task<bool> RemoveFromRoleAsync(UserRole role)
    {
        var user = await GetMyUserEntityAsync();
        var identityResult = await _userManager.RemoveFromRoleAsync(user, role.ToString());

        if (!identityResult.Succeeded)
            _errors.ThrowRemoveUserRoleException([..identityResult.Errors]);

        await UpdatedUserEntityNow(user);
        await RegenerateUserCookieAsync();

        return true;
    }

    public async Task<bool> UpdateEmailAsync(UserDtoEmailUpdate dtoRequest)
    {
        var user = await GetMyUserEntityAsync();
        var identityResult = await _userManager.SetEmailAsync(user, dtoRequest.Email);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdateEmailException([..identityResult.Errors]);

        await UpdatedUserEntityNow(user);
        return true;
    }

    public async Task<bool> UpdatePasswordAsync(UserDtoPasswordUpdate dtoRequest)
    {
        var user = await GetMyUserEntityAsync();
        var identityResult = await _userManager.ChangePasswordAsync(user, dtoRequest.CurrentPassword, dtoRequest.NewPassword);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdatePasswordException([..identityResult.Errors]);

        await UpdatedUserEntityNow(user);
        return true;
    }

    public async Task<bool> UpdatePhoneNumberAsync(UserDtoPhoneNumberUpdate dtoRequest)
    {
        var user = await GetMyUserEntityAsync();
        var identityResult = await _userManager.SetPhoneNumberAsync(user, dtoRequest.PhoneNumber);

        if (!identityResult.Succeeded)
            _errors.ThrowUpdatePhoneNumberException([..identityResult.Errors]);

        await UpdatedUserEntityNow(user);
        return true;
    }

    private async Task DeleteUserEntity(User user)
    {
        user.DeleteEntity();
        await _userManager.UpdateAsync(user);
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await GetMeAsync();

        if (user is null)
            _errors.ThrowNotFoundException();

        if (user!.Id != id)
            _errors.ThrowWrongUserIdException(id);

        using var transaction = await BeginTransactionAsync();

        try
        {
            string[] roles = Enum.GetNames(typeof(UserRole));
            var roleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!roleResult.Succeeded)
                _errors.ThrowBasicUserException([..roleResult.Errors]);

            await DeleteUserEntity(user);

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
