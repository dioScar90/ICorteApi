using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IServiceAppointmentRepository
    : IBaseCompositeKeyRepository<ServiceAppointment, int, int>
{
}
