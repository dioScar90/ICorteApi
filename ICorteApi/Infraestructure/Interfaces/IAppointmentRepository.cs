using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IAppointmentRepository
    : IBaseRepository<Appointment>
{
    Task<Appointment?> GetByIdWithServicesAsync(int id);
    Task<Appointment[]> GetAppointmentsByDateAsync(int barberShopId, DateOnly date);
}
