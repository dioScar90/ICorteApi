using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IRecurringScheduleService : IService<RecurringSchedule>
{
    Task<ISingleResponse<RecurringSchedule>> CreateAsync(IDtoRequest<RecurringSchedule> dtoRequest, int barberShopId);
    Task<ISingleResponse<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
    Task<ICollectionResponse<RecurringSchedule>> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<IResponse> UpdateAsync(IDtoRequest<RecurringSchedule> dtoRequest, DayOfWeek dayOfWeek, int barberShopId);
    Task<IResponse> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId);
}
