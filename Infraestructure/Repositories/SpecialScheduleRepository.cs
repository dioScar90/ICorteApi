namespace ICorteApi.Infraestructure.Repositories;

public sealed class SpecialScheduleRepository(AppDbContext context)
    : BaseRepository<SpecialSchedule>(context), ISpecialScheduleRepository
{
}
