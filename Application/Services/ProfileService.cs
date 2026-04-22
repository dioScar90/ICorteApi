using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    AppDbContext context,
    UserService userService)
    : BaseService<Profile>(context)
{
    public async Task<ProfileDtoResponse?> CreateAsync(ProfileDtoRequest dto)
    {
        var userId = await userService.GetMyUserIdAsync();
        var profile = new Profile(dto, userId);
        
        using var transaction = await BeginTransactionAsync();
        
        try
        {
            dbSet.Add(profile);
            
            await userService.AddUserRoleAsync(new(profile.User.Id, UserRole.Client));
            await userService.UpdatePhoneNumberAsync(new(profile.User.Id, profile.User.PhoneNumber!));
            
            await transaction.CommitAsync();
            return profile.CreateDto();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ProfileExistsAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Id == id);
    }
    
    public async Task<bool> ProfileIsMineAsync(int id)
    {
        var userId = await userService.GetMyUserIdAsync();

        return id == userId && await dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Id == userId);
    }
    
    public async Task<ProfileDtoResponse?> GetByIdAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProfileDtoResponse(
                p.Id,
                p.FirstName,
                p.LastName,
                p.FirstName + ' ' + p.LastName,
                p.Gender,
                p.ImageUrl
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> UpdateAsync(ProfileDtoRequest dto, int id)
    {
        var profile = await dbSet.FindAsync(id);

        if (profile is null)
            return false;
            
        profile.UpdateEntity(dto);
        
        using var transaction = await BeginTransactionAsync();
        
        try
        {
            dbSet.Update(profile);
            await userService.UpdatePhoneNumberAsync(new(profile.User.Id, profile.User.PhoneNumber!));

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
