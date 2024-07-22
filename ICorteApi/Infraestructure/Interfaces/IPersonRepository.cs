using System.Linq.Expressions;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IPersonRepository
{
    Task<IResponse> CreateAsync(Person person);
    Task<ISingleResponse<Person>> GetByIdAsync(int userId);
    Task<ICollectionResponse<Person>> GetAllAsync(int page, int pageSize, Expression<Func<Person, bool>>? filter = null);
    Task<IResponse> UpdateAsync(Person person);
    Task<IResponse> DeleteAsync(int userId);
}
