using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IRecurringScheduleService
{
       Task<IResponseModel> CreateAsync(RecurringScheduleDtoRequest dto);
       Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(int id);
       Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync();
       Task<IResponseModel> UpdateAsync(RecurringScheduleDtoRequest dto);
       Task<IResponseModel> DeleteAsync(int id);
}
