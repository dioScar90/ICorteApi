using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    IProfileRepository repository,
    IValidator<ProfileDtoCreate> createValidator,
    IValidator<ProfileDtoUpdate> updateValidator,
    IProfileErrors errors)
    : BaseService<Profile>(repository), IProfileService
{
    new private readonly IProfileRepository _repository = repository;
    private readonly IValidator<ProfileDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ProfileDtoUpdate> _updateValidator = updateValidator;
    private readonly IProfileErrors _errors = errors;

    public async Task<ProfileDtoResponse> CreateAsync(ProfileDtoCreate dto, int userId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var profile = new Profile(dto, userId);
        return (await _repository.CreateAsync(profile, profile.GetPhoneNumberToUserEntity()))!.CreateDto();
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
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var profile = await GetByIdAsync(id);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        profile.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(profile, dto.PhoneNumber);
    }
}
