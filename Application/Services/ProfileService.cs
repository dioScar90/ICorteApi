using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    AppDbContext context,
    ILogger<ProfileService> _logger,
    UserService _userService,
    ProfileErrors _errors)
    : BaseService<Profile>(context)
{
    public async Task<ProfileDtoResponse> CreateAsync(ProfileDtoRequest dto)
    {
        var userId = await _userService.GetMyUserIdAsync();
        var profile = new Profile(dto, userId);
        
        using var transaction = await BeginTransactionAsync();

        try
        {
            _dbSet.Add(profile);
            
            await _userService.AddUserRoleAsync(UserRole.Client);
            await _userService.UpdatePhoneNumberAsync(new(profile.User.PhoneNumber!));

            await transaction.CommitAsync();
            return await GetByIdAsync(profile.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ProfileDtoResponse> GetByIdAsync(int id)
    {
        var userId = await _userService.GetMyUserIdAsync();

        var profile = await _dbSet
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

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        return profile;
    }
    
    public async Task UpdateAsync(ProfileDtoRequest dto, int id)
    {
        var userId = await _userService.GetMyUserIdAsync();
        var profile = await _dbSet.FindAsync(id);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        profile.UpdateEntity(dto);

        using var transaction = await BeginTransactionAsync();

        try
        {
            _dbSet.Update(profile);
            await _userService.UpdatePhoneNumberAsync(new(profile.User.PhoneNumber!));

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
