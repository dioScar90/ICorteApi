using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IPersonService
    : IBasePrimaryKeyService<Person, int>
{
}
