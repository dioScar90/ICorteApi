using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IPersonRepository
{
    Task<IResponseModel> CreateAsync(Person person);
    Task<IResponseDataModel<Person>> GetByIdAsync(int userId);
    Task<IResponseDataModel<ICollection<Person>>> GetAllAsync(int page, int pageSize, Expression<Func<Person, bool>>? filter = null);
    Task<IResponseModel> UpdateAsync(Person person);
    Task<IResponseModel> DeleteAsync(int userId);
}
