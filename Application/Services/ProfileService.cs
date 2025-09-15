using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    AppDbContext context,
    IValidator<ProfileDtoCreate> createValidator,
    IValidator<ProfileDtoUpdate> updateValidator,
    UserService userService,
    IProfileErrors errors)
    : BaseService<Profile>(context), IProfileService
{
    private readonly IValidator<ProfileDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ProfileDtoUpdate> _updateValidator = updateValidator;
    private readonly UserService _userService = userService;
    private readonly IProfileErrors _errors = errors;

    public async Task<ProfileDtoResponse> CreateAsync(ProfileDtoCreate dto, int userId)
    {
        dto.ThrowExceptionIfInvalid(_createValidator, _errors);
        var profile = new Profile(dto, userId);
        
        using var transaction = await BeginTransactionAsync();

        try
        {
            var newProfile = await CreateAsync(profile);

            if (newProfile is null)
                _errors.ThrowCreateException();

            await _userService.AddUserRoleAsync(UserRole.Client);
            await _userService.UpdatePhoneNumberAsync(new(dto.PhoneNumber));

            await CommitAsync(transaction);
            return newProfile!.CreateDto();
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public async Task<ProfileDtoResponse> GetByIdAsync(int id, int userId)
    {
        var profile = await GetByIdAsync(id);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        return profile.CreateDto();
    }
    
    public async Task<bool> UpdateAsync(ProfileDtoUpdate dto, int id, int userId)
    {
        dto.ThrowExceptionIfInvalid(_updateValidator, _errors);

        var profile = await GetByIdAsync(id);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        profile.UpdateEntityByDto(dto);

        using var transaction = await BeginTransactionAsync();

        try
        {
            _dbSet.Update(profile);
            await _userService.UpdatePhoneNumberAsync(new(dto.PhoneNumber));

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
