using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface ISpecialScheduleRepository
    : IBaseCompositeKeyRepository<SpecialSchedule, DateOnly, int>
{
}
