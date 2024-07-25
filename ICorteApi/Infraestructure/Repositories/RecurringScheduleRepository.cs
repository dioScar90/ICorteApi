using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class RecurringScheduleRepository(AppDbContext context)
    : BaseCompositeKeyRepository<RecurringSchedule, DayOfWeek, int>(context), IRecurringScheduleRepository
{
}
