using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IServiceAppointmentService : IService<ServiceAppointment>
{
    Task<ISingleResponse<ServiceAppointment>> GetByIdAsync(int appointmentId, int serviceId);
    Task<IResponse> UpdateAsync(IDtoRequest<ServiceAppointment> dtoRequest, int appointmentId, int serviceId);
    Task<IResponse> DeleteAsync(int appointmentId, int serviceId);
}
