namespace ICorteApi.Infraestructure.Repositories;

public sealed class RecurringScheduleRepository(AppDbContext context)
    : BaseRepository<RecurringSchedule>(context), IRecurringScheduleRepository
{
}
