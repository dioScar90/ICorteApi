using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(IProfileRepository repository, IImageService imageService, IProfileErrors errors)
    : BaseService<Profile>(repository), IProfileService
{
    new private readonly IProfileRepository _repository = repository;
    private readonly IImageService _imageService = imageService;
    private readonly IProfileErrors _errors = errors;

    public async Task<Profile?> CreateAsync(ProfileDtoCreate dto, int userId)
    {
        var profile = new Profile(dto, userId);
        return await _repository.CreateAsync(profile, dto.PhoneNumber);
    }

    public async Task<Profile?> GetByIdAsync(int id, int userId)
    {
        var profile = await GetByIdAsync(id);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        return profile;
    }

    public async Task<bool> UpdateAsync(ProfileDtoUpdate dtoRequest, int id, int userId)
    {
        var profile = await GetByIdAsync(id, userId);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        profile.UpdateEntityByDto(dtoRequest);
        return await UpdateAsync(profile);
    }

    public async Task<bool> UpdateProfileImageAsync(int id, int userId, IFormFile image)
    {
        var profile = await GetByIdAsync(id, userId);

        if (profile is null)
            _errors.ThrowNotFoundException();

        if (profile!.Id != userId)
            _errors.ThrowProfileNotBelongsToUserException(userId);

        // Salve a imagem no sistema de arquivos ou em um serviço de nuvem
        string imageUrl = await _imageService.SaveImageAsync(image);

        profile.UpdateImageUrl(imageUrl);
        return await _repository.UpdateAsync(profile);
    }
}
