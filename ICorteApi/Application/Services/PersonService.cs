using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository) : IPersonService
{
    private readonly IPersonRepository _repository = personRepository;

    public async Task<IResponse> CreateAsync(Person person)
    {
        return await _repository.CreateAsync(person);
    }

    public async Task<IResponse> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<ICollectionResponse<Person>> GetAllAsync(int page, int pageSize)
    {
        return await _repository.GetAllAsync(1, 25);
    }

    public async Task<ISingleResponse<Person>> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IResponse> UpdateAsync(int id, PersonDtoRequest dto)
    {
        var response = await _repository.GetByIdAsync(id);

        if (!response.IsSuccess)
            return response;

        var person = response.Value!;

        person.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(person);
    }
}
