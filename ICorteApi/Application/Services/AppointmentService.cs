using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class AppointmentService(IAppointmentRepository appointmentRepository)
    : BasePrimaryKeyService<Appointment, int>(appointmentRepository), IAppointmentService
{
}
