using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IPersonService
    : IBasePrimaryKeyService<Person, int>, IHasOneForeignKeyService<Person, int>
{
    new Task<ISingleResponse<Person>> CreateAsync(IDtoRequest<Person> dtoRequest, int userId);
}
