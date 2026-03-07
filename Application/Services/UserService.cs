using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class UserService(
        IHttpContextAccessor _httpCtx,
        UserManager<User> _userManager,
        SignInManager<User> _signInManager,
        AppDbContext _context,
        UserErrors _userErrors)
{
    private readonly DbSet<User> _userDbSet = _context.Set<User>();
    
    private async Task<User?> GetMyUserEntityAsync()
    {
        if (_httpCtx.HttpContext?.User is null)
            return null;
            
        var user = await _userManager.GetUserAsync(_httpCtx.HttpContext.User);
        
        return user;
    }
    
    private async Task<UserDtoResponse?> GetMyUserDtoAsync() => (await GetMyUserEntityAsync())?.CreateDto();
    
    private async Task RegenerateUserCookieAsync(User? user = null) =>
        await _signInManager.RefreshSignInAsync(user ?? await GetMyUserEntityAsync());
        
    public async Task<int?> GetMyUserIdAsync()
    {
        var user = await GetMyUserDtoAsync();
        return user?.Id;
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
    
    public async Task<IdentityResult?> CreateAsync(UserDtoRegisterRequest dto)
    {
        var newUser = new User(dto);
        
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await _userManager.CreateAsync(newUser, newUser.GetPasswordToBeHashed());

            if (!result.Succeeded)
                return result;
                
            result = await _userManager.AddToRolesAsync(newUser, [..GetUserRolesToBeSetted(newUser)]);

            if (!result.Succeeded)
                return result;
            
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<UserDtoResponse?> GetMeAsync(bool? dispatchIncludes = false)
    {
        if (dispatchIncludes == true)
            return await GetMyUserDtoAsync();

        var userId = await GetMyUserIdAsync();

        if (userId is null)
            return null;

        var user = await _userDbSet
            .AsNoTracking()
            .Include(u => u.Profile)
            .Include(u => u.BarberShop)
            .AsSplitQuery()
            .Select(u => new UserDtoResponse(
                u.Id,
                u.Email,
                u.PhoneNumber,
                Array.Empty<string>(),
                new ProfileDtoResponse(
                    u.Profile.Id,
                    u.Profile.FirstName,
                    u.Profile.LastName,
                    u.Profile.FirstName + " " + u.Profile.LastName,
                    u.Profile.Gender,
                    u.Profile.ImageUrl
                ),
                new BarberShopDtoResponse(
                    u.BarberShop.Id,
                    u.BarberShop.OwnerId,
                    u.BarberShop.Name,
                    u.BarberShop.Description,
                    u.BarberShop.ComercialNumber,
                    u.BarberShop.ComercialEmail,
                    null,
                    Array.Empty<RecurringScheduleDtoResponse>(),
                    Array.Empty<SpecialScheduleDtoResponse>(),
                    Array.Empty<ServiceDtoResponse>(),
                    Array.Empty<ReportDtoResponse>()
                )
            ))
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return null;
            
        var roles = await GetUserRolesAsync();

        if (roles is null)
            return user;
        
        return user with { Roles = [..roles.Select(r => r.ToString())] };
    }
    
    public async Task<IdentityResult?> AddUserRoleAsync(UserDtoAddRoleRequest dto)
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            return null;
        
        var result = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
        
        if (!result.Succeeded)
            return result;
            
        await RegenerateUserCookieAsync();
        return result;
    }
    
    public async Task<IdentityResult?> RemoveFromRoleAsync(UserDtoRemoveRoleRequest dto)
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            return null;
        
        var result = await _userManager.RemoveFromRoleAsync(user, dto.Role.ToString());

        if (!result.Succeeded)
            return result;
            
        if (!result.Succeeded)
            return result;
            
        user.UpdatedUserNow();

        result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return result;
            
        await RegenerateUserCookieAsync();
        return result;
    }
    
    public async Task<IdentityResult?> UpdateEmailAsync(UserDtoEmailUpdate dtoRequest)
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            return null;
        
        var result = await _userManager.SetEmailAsync(user, dtoRequest.Email);
        
        if (!result.Succeeded)
            return result;
            
        user.UpdatedUserNow();

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult?> UpdatePasswordAsync(UserDtoPasswordUpdateRequest dtoRequest)
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            return null;

        var result = await _userManager.ChangePasswordAsync(user, dtoRequest.CurrentPassword, dtoRequest.NewPassword);

        if (!result.Succeeded)
            return result;
            
        user.UpdatedUserNow();

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult?> UpdatePhoneNumberAsync(UserDtoPhoneNumberUpdate dtoRequest)
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            return null;

        var result = await _userManager.SetPhoneNumberAsync(user, dtoRequest.PhoneNumber);

        if (!result.Succeeded)
            return result;
            
        user.UpdatedUserNow();

        return await _userManager.UpdateAsync(user);
    }
    
    public async Task<bool> IsUserFromGivenId(int? id)
    {
        if (id is not > 0)
            return false;

        var realId = await GetMyUserIdAsync();

        return realId == id;
    }
    
    public async Task<IdentityResult?> DeleteAsync(int id)
    {
        var user = await GetMyUserEntityAsync();

        if (user is null)
            return null;

        if (user!.Id != id)
            return null;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            string[] roles = Enum.GetNames(typeof(UserRole));

            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
                return result;

            user.DeleteEntity();

            result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return result;
                
            result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return result;

            await transaction.CommitAsync();
            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
