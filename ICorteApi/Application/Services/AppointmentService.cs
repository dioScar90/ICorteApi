using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository)
    : BasePrimaryKeyService<Appointment, int>(repository), IAppointmentService
{
}
