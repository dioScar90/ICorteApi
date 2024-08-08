using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IRecurringScheduleService
    : IBaseCompositeKeyService<RecurringSchedule, DayOfWeek, int>, IHasOneForeignKeyService<RecurringSchedule, int>
{
}
