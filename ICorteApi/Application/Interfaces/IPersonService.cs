using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IPersonService
    : IBasePrimaryKeyService<Person, int>
{
    Task<IResponse> CreateAsync(int userId, PersonDtoRequest dto);
}
