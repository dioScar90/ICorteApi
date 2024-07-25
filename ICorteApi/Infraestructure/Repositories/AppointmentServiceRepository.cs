using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class AppointmentServiceRepository(AppDbContext context)
    : BaseCompositeKeyRepository<AppointmentService, int, int>(context), IAppointmentServiceRepository
{
}
