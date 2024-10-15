using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(
    IProfileRepository repository,
    IImageService imageService,
    IValidator<ProfileDtoCreate> createValidator,
    IValidator<ProfileDtoUpdate> updateValidator,
    IProfileErrors errors)
    : BaseService<Profile>(repository), IProfileService
{
    new private readonly IProfileRepository _repository = repository;
    private readonly IValidator<ProfileDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ProfileDtoUpdate> _updateValidator = updateValidator;
    private readonly IImageService _imageService = imageService;
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
        return await UpdateAsync(profile);
    }

    private static bool ImageIsNullOrHasNoLength(IFormFile? image) => image is null or { Length: 0 };

    public async Task<bool> UpdateProfileImageAsync(int id, int userId, IFormFile? image)
    {
        if (ImageIsNullOrHasNoLength(image))
            // _errors.ThrowUpdateException("A imagem fornecida é inválida.");
            _errors.ThrowUpdateException();
        
        var profile = await GetByIdAsync(id);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        // Salve a imagem no sistema de arquivos ou em um serviço de nuvem
        string imageUrl = await _imageService.SaveImageAsync(image);

        // profile.UpdateImageUrl(imageUrl);
        return await _repository.UpdateAsync(profile);
    }
}
