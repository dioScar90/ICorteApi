using System.Linq.Expressions;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IScheduleRepository
{
    Task<IResponseModel> CreateAsync(Schedule schedule);
    Task<IResponseDataModel<Schedule>> GetByIdAsync(int id);
    Task<IResponseDataModel<ICollection<Schedule>>> GetAllAsync(int page, int pageSize, Expression<Func<Schedule, bool>>? filter = null);
    Task<IResponseModel> UpdateAsync(Schedule schedule);
    Task<IResponseModel> DeleteAsync(int id);
}
