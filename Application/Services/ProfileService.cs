using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    AppDbContext context,
    UserService userService)
    : BaseService<Profile>(context)
{
    public async Task<ProfileDtoResponse?> CreateAsync(
        ProfileDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userId = await userService.GetMyUserIdAsync();
        var profile = new Profile(dto, userId);
        
        using var transaction = await BeginTransactionAsync(cancellationToken);
        
        try
        {
            dbSet.Add(profile);
            
            await userService.AddUserRoleAsync(new(profile.User.Id, UserRole.Client));
            await userService.UpdatePhoneNumberAsync(new(profile.User.Id, profile.User.PhoneNumber!), cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            return profile.CreateDto();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task<EntityInfos> GetEntityInfosAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUserId = await userService.GetMyUserIdAsync();
        
        var infos = await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Select(p => new EntityInfos(
                p.DeletedAt == null,
                currentUserId != null && p.Id == currentUserId
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return infos ?? new();
    }
    
    public async Task<ProfileDtoResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<ProfileDtoResponse?> UpdateAsync(
        ProfileDtoRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var profile = await dbSet.FindAsync([id], cancellationToken);

        if (profile is null)
            return null;
            
        profile.UpdateEntity(dto);
        
        using var transaction = await BeginTransactionAsync(cancellationToken);
        
        try
        {
            dbSet.Update(profile);
            await userService.UpdatePhoneNumberAsync(new(profile.User.Id, profile.User.PhoneNumber!), cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return profile.CreateDto();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
