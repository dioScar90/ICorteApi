using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRecurringScheduleRepository
    : IBaseRepository<RecurringSchedule>
{
    Task<DayOfWeek[]> GetAvailableDaysForBarberAsync(int barberShopId, DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek);
}
