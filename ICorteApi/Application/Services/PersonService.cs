using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository) : IPersonService
{
    private readonly IPersonRepository _repository = personRepository;

    public async Task<IResponse> CreateAsync(PersonDtoRequest dto)
    {
        var person = dto.CreateEntity()!;
        return await _repository.CreateAsync(person);
    }

    public async Task<IResponse> DeleteAsync(int id)
    {
        var response = await _repository.GetByIdAsync(x => x.UserId == id);

        if (!response.IsSuccess)
            return Response.Failure(Error.PersonNotFound);
        
        return await _repository.DeleteAsync(response.Value!);
    }

    public async Task<ICollectionResponse<Person>> GetAllAsync(int page, int pageSize)
    {
        return await _repository.GetAllAsync(page, pageSize);
    }

    public async Task<ISingleResponse<Person>> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(x => x.UserId == id);
    }

    public async Task<IResponse> UpdateAsync(int id, PersonDtoRequest dto)
    {
        var response = await _repository.GetByIdAsync(x => x.UserId == id);

        if (!response.IsSuccess)
            return Response.Failure(Error.PersonNotFound);

        var person = response.Value!;

        person.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(person);
    }
}
