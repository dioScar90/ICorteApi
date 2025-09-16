namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService : IService<Appointment>
{
    Task<AppointmentDto> CreateAsync(AppointmentDto dto, int clientId);
    Task<AppointmentDto> GetByIdAsync(int id);
    Task<AppointmentDto> GetByIdWithServicesAsync(int id);
    Task<PaginationResponse<AppointmentDto>> GetAllAsync(int? page, int? pageSize, int clientId);
    Task<bool> UpdateAsync(AppointmentDto dto, int id, int clientId);
    Task<bool> UpdatePaymentTypeAsync(AppointmentPaymentTypeDtoUpdate dto, int id, int clientId);
    Task<bool> DeleteAsync(int id, int clientId);
}
