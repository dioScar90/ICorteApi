using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IAppointmentServiceRepository
    : IBaseCompositeKeyRepository<AppointmentService, int, int>
{
}
