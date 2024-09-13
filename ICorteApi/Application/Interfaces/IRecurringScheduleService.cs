namespace ICorteApi.Application.Interfaces;

public interface IRecurringScheduleService : IService<RecurringSchedule>
{
    Task<RecurringScheduleDtoResponse> CreateAsync(RecurringScheduleDtoCreate dto, int barberShopId);
    Task<RecurringScheduleDtoResponse> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
    Task<RecurringScheduleDtoResponse[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(RecurringScheduleDtoUpdate dto, DayOfWeek dayOfWeek, int barberShopId);
    Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId);
}
