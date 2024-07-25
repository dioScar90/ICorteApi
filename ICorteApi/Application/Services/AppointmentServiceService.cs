using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class AppointmentServiceService(IAppointmentServiceRepository appointmentServiceRepository)
    : BaseCompositeKeyService<Domain.Entities.AppointmentService, int, int>(appointmentServiceRepository), IAppointmentServiceService
{
}
