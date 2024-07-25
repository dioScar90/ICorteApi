using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService
    : IBasePrimaryKeyService<Appointment, int>
{
}
