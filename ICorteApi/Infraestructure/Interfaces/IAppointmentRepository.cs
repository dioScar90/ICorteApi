using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IAppointmentRepository
    : IBasePrimaryKeyRepository<Appointment, int>
{
}
