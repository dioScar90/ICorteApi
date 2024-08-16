using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class PersonService(IPersonRepository repository)
    : BasePrimaryKeyService<Person, int>(repository), IPersonService
{
    new private readonly IPersonRepository _repository = repository;
    
    public async Task<ISingleResponse<Person>> CreateAsync(IDtoRequest<Person> dtoRequest, int userId)
    {
        if (dtoRequest is not PersonDtoRegisterRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var person = new Person(dto, userId);
        return await _repository.CreateAsync(person, dto.PhoneNumber);
    }

    public override async Task<IResponse> DeleteAsync(int id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;
            
        return await _repository.DeleteAsync(resp.Value!);
    }
}
