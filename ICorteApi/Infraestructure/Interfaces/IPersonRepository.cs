using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IPersonRepository
    : IBasePrimaryKeyRepository<Person, int>
{
}
