using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IScheduleService
{
       Task<IResponseModel> CreateAsync(ScheduleDtoRequest dto);
       Task<IResponseDataModel<Schedule>> GetByIdAsync(int id);
       Task<IResponseDataModel<ICollection<Schedule>>> GetAllAsync();
       Task<IResponseModel> UpdateAsync(ScheduleDtoRequest dto);
       Task<IResponseModel> DeleteAsync(int id);
}
