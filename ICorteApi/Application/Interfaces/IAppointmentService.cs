using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService : IService<Appointment>
{
    Task<Appointment?> CreateAsync(AppointmentDtoRequest dto, int clientId);
    Task<Appointment?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(AppointmentDtoRequest dto, int id, int clientId);
    Task<bool> DeleteAsync(int id, int clientId);
}
