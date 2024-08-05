using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ServiceAppointmentRepository(AppDbContext context)
    : BaseCompositeKeyRepository<ServiceAppointment, int, int>(context), IServiceAppointmentRepository
{
}
