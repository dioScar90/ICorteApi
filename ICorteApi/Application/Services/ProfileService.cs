using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ProfileService(IProfileRepository repository)
    : BaseService<Profile>(repository), IProfileService
{
    new private readonly IProfileRepository _repository = repository;

    public async Task<ISingleResponse<Profile>> CreateAsync(IDtoRequest<Profile> dtoRequest, int userId)
    {
        if (dtoRequest is not ProfileDtoRegisterRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Profile(dto, userId);
        return await _repository.CreateAsync(entity, dto.PhoneNumber);
    }
    
    public async Task<ISingleResponse<Profile>> GetByIdAsync(int id, int userId)
    {
        var resp = await GetByIdAsync(id);
        
        if (!resp.IsSuccess)
            return resp;

        if (resp.Value!.Id != userId)
            return Response.Failure<Profile>(Error.TEntityNotFound);

        return resp;
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<Profile> dtoRequest, int id, int userId)
    {
        var resp = await GetByIdAsync(id, userId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }
}
