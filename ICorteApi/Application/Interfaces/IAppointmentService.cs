namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService : IService<Appointment>
{
    Task<AppointmentDtoResponse> CreateAsync(AppointmentDtoCreate dto, int clientId);
    Task<AppointmentDtoResponse> GetByIdAsync(int id);
    Task<bool> UpdateAsync(AppointmentDtoUpdate dto, int id, int clientId);
    Task<bool> DeleteAsync(int id, int clientId);
}
