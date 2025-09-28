using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    AppDbContext context,
    UserService userService,
    ProfileValidator validator,
    ProfileErrors errors)
    : BaseService<Profile, ProfileDtoResponse, ProfileDtoRequest>(context, userService)
{
    private readonly ProfileValidator _validator = validator;
    private readonly ProfileErrors _errors = errors;

    public override async Task<ProfileDtoResponse> CreateAsync(ProfileDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

        var userId = await _userService.GetMyUserIdAsync();
        var profile = new Profile(dto, userId);
        
        using var transaction = await BeginTransactionAsync();

        try
        {
            _dbSet.Add(profile);
            // var newProfile = await CreateAsync(profile!);

            // if (newProfile is null)
            //     _errors.ThrowCreateException();

            await _userService.AddUserRoleAsync(UserRole.Client);
            await _userService.UpdatePhoneNumberAsync(new(profile.User.PhoneNumber!));

            await CommitAsync(transaction);
            return await GetByIdAsync(profile.Id);
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
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
    
    public async Task<bool> UpdateAsync(ProfileDtoRequest dto, int id)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

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
