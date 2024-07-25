using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository)
    : BasePrimaryKeyService<Person, int>(personRepository), IPersonService
{
    private readonly IPersonRepository _primaryPersonRepository = personRepository;

    public async Task<IResponse> CreateAsync(int userId, PersonDtoRequest dto)
    {
        var newPerson = dto.CreateEntity()!;
        newPerson.UserId = userId;

        return await _primaryPersonRepository.CreateAsync(newPerson);
    }
}
