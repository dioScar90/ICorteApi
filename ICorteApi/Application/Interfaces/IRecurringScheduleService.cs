using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IRecurringScheduleService : IService<RecurringSchedule>
{
    Task<RecurringSchedule?> CreateAsync(RecurringScheduleDtoRequest dto, int barberShopId);
    Task<RecurringSchedule?> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
    Task<RecurringSchedule[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(RecurringScheduleDtoRequest dto, DayOfWeek dayOfWeek, int barberShopId);
    Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId);
}
