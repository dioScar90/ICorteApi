using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface ISpecialScheduleService
    : IBaseCompositeKeyService<SpecialSchedule, DateOnly, int>, IHasOneForeignKeyService<SpecialSchedule, int>
{
}
