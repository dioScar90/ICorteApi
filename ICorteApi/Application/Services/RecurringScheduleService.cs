using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class RecurringScheduleService(IRecurringScheduleRepository recurringScheduleRepository)
    : BaseCompositeKeyService<RecurringSchedule, DayOfWeek, int>(recurringScheduleRepository), IRecurringScheduleService
{
}
