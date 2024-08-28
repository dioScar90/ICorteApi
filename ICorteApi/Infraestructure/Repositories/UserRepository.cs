using System.Security.Claims;
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

public sealed class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _context;
    private readonly DbSet<User> _dbSet;

    private readonly int? _userId;
    private readonly User? _user;
    
    public UserRepository(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
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

    }

    private async Task RegenerateUserCookieAsync(User user) => await _signInManager.RefreshSignInAsync(user);

    public async Task<ISingleResponse<User>> CreateUserAsync(User newUser, string password)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var userResult = await _userManager.CreateAsync(newUser, password);

            if (!userResult.Succeeded)
            {
                foreach (var err in userResult.Errors)
                    errors.Add(new(err.Code, err.Description));
                
                throw new Exception();
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, nameof(UserRole.Guest));

            if (!roleResult.Succeeded)
            {
                foreach (var err in userResult.Errors)
                    errors.Add(new(err.Code, err.Description));
                
                throw new Exception();
            }

            await RegenerateUserCookieAsync(newUser);
        
            await transaction.CommitAsync();
            return Response.Success(newUser);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<User>([..errors]);
    }
    
    public int? GetMyUserId() => _userId;
    
    public async Task<UserRole[]> GetUserRolesAsync()
    {
        if (_user is null)
            return [];

        var roles = await _userManager.GetRolesAsync(_user);

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
        var userId = GetMyUserId();

        if (userId is not int id)
            return Response.Failure<User>(Error.UserNotFound);

        var userEntity = await _dbSet
            .Include(u => u.Profile)
            .Include(u => u.OwnedBarberShop)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (userEntity is not User user)
            return Response.Failure<User>(Error.UserNotFound);

        var roles = await GetUserRolesAsync() ?? [];
        user.SetRoles(roles);

        return Response.Success(user);
    }

    private async void UpdatedUserEntityNow(User user)
    {
        user.UpdatedUserNow();
        await _userManager.UpdateAsync(user);
    }

    public async Task<IResponse> AddUserRoleAsync(UserRole role)
    {
        var res = await _userManager.AddToRoleAsync(_user!, Enum.GetName(role)!);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return Response.Failure(errors);
        }
        
        UpdatedUserEntityNow(_user!);
        
        await RegenerateUserCookieAsync(_user!);
        return Response.Success();
    }

    public async Task<IResponse> RemoveFromRoleAsync(UserRole role)
    {
        var res = await _userManager.RemoveFromRoleAsync(_user!, Enum.GetName(role)!);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return Response.Failure(errors);
        }

        UpdatedUserEntityNow(_user!);
        
        await RegenerateUserCookieAsync(_user!);
        return Response.Success();
    }

    public async Task<IResponse> UpdateEmailAsync(string newEmail)
    {
        var res = await _userManager.SetEmailAsync(_user!, newEmail);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return Response.Failure(errors);
        }

        UpdatedUserEntityNow(_user!);
        return Response.Success();
    }

    public async Task<IResponse> UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        var res = await _userManager.ChangePasswordAsync(_user!, currentPassword, newPassword);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return Response.Failure(errors);
        }

        UpdatedUserEntityNow(_user!);
        return Response.Success();
    }

    public async Task<IResponse> UpdatePhoneNumberAsync(string newPhoneNumber)
    {
        var res = await _userManager.SetPhoneNumberAsync(_user!, newPhoneNumber);

        if (!res.Succeeded)
        {
            var errors = res.Errors.Select(err => new Error(err.Code, err.Description)).ToArray();
            return Response.Failure(errors);
        }

        UpdatedUserEntityNow(_user!);
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
        List<Error> errors = [];

        try
        {
            string[] roles = Enum.GetNames(typeof(UserRole));
            var roleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!roleResult.Succeeded)
            {
                errors.AddRange(roleResult.Errors.Select(err => new Error(err.Code, err.Description)));
                throw new Exception();
            }
            
            DeleteUserEntity(user);
            var res = await _userManager.DeleteAsync(user);

            if (!res.Succeeded)
            {
                errors.AddRange(res.Errors.Select(err => new Error(err.Code, err.Description)));
                throw new Exception();
            }
            
            await transaction.CommitAsync();
            return Response.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<User>([..errors]);
    }
}
