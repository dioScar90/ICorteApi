using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository)
    : BasePrimaryKeyService<Person, int>(personRepository), IPersonService
{
}
