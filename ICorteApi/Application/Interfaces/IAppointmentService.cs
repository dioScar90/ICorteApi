namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService : IService<Appointment>
{
    Task<AppointmentDtoResponse> CreateAsync(AppointmentDtoCreate dto, int clientId);
    Task<AppointmentDtoResponse> GetByIdAsync(int id);
    Task<PaginationResponse<AppointmentDtoResponse>> GetAllAsync(int? page, int? pageSize, int clientId);
    Task<bool> UpdateAsync(AppointmentDtoUpdate dto, int id, int clientId);
    Task<bool> DeleteAsync(int id, int clientId);
}
