using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IAppointmentRepository
    : IBaseRepository<Appointment>
{
    Task<ISingleResponse<Appointment>> GetByIdWithServicesAsync(int id);
}
