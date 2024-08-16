using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IPersonRepository
    : IBasePrimaryKeyRepository<Person, int>
{
    Task<ISingleResponse<Person>> CreateAsync(Person person, string phoneNumber);
}
