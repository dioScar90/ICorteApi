using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(IProfileRepository repository)
    : BasePrimaryKeyService<Profile, int>(repository), IProfileService
{
    new private readonly IProfileRepository _repository = repository;

    public async Task<ISingleResponse<Profile>> CreateAsync(IDtoRequest<Profile> dtoRequest, int userId)
    {
        if (dtoRequest is not ProfileDtoRegisterRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var profile = new Profile(dto, userId);
        return await _repository.CreateAsync(profile, dto.PhoneNumber);
    }

    public override async Task<IResponse> DeleteAsync(int id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;

        return await _repository.DeleteAsync(resp.Value!);
    }
}
