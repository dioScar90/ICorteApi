using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class SpecialScheduleService(ISpecialScheduleRepository specialScheduleRepository)
    : BaseCompositeKeyService<SpecialSchedule, DateOnly, int>(specialScheduleRepository), ISpecialScheduleService
{
}
