using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ServiceAppointmentRepository(AppDbContext context)
    : BaseRepository<ServiceAppointment>(context), IServiceAppointmentRepository
{
}
