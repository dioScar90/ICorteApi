using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IRecurringScheduleService
{
       Task<IResponse> CreateAsync(int barberShopId, RecurringScheduleDtoRequest dto);
       Task<IResponse> CreateManyAsync(RecurringScheduleDtoRequest[] dtoArr);
       Task<ISingleResponse<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
       Task<ICollectionResponse<RecurringSchedule>> GetAllAsync(int barberShopId);
       Task<IResponse> UpdateAsync(int barberShopId, RecurringScheduleDtoRequest dto);
       Task<IResponse> UpdateManyAsync(int barberShopId, RecurringScheduleDtoRequest[] dtoArr);
       Task<IResponse> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId);
}
