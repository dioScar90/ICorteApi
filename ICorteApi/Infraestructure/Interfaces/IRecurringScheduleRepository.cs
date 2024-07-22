using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRecurringScheduleRepository
{
    Task<IResponse> CreateAsync(RecurringSchedule recurringSchedule);
    Task<ISingleResponse<RecurringSchedule>> GetByIdAsync(int barberShopId, DayOfWeek dayOfWeek);
    Task<ICollectionResponse<RecurringSchedule>> GetAllAsync(int barberShopId);
    Task<IResponse> UpdateAsync(RecurringSchedule recurringSchedule);
    Task<IResponse> DeleteAsync(RecurringSchedule recurringSchedule);
}
