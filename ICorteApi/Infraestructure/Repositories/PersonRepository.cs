using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class PersonRepository(AppDbContext context)
    : BasePrimaryKeyRepository<Person, int>(context), IPersonRepository
{
}
