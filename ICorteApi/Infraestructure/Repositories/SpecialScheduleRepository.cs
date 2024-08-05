using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class SpecialScheduleRepository(AppDbContext context)
    : BaseCompositeKeyRepository<SpecialSchedule, DateOnly, int>(context), ISpecialScheduleRepository
{
}
