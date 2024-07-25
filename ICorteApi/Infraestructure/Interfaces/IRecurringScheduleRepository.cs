using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRecurringScheduleRepository
    : IBaseCompositeKeyRepository<RecurringSchedule, DayOfWeek, int>
{
}
