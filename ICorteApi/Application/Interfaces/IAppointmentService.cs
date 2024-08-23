using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService : IService<Appointment>
{
    Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dtoRequest, int clientId);
    Task<ISingleResponse<Appointment>> GetByIdAsync(int id, int clientId);
    Task<IResponse> UpdateAsync(IDtoRequest<Appointment> dtoRequest, int id, int clientId);
    Task<IResponse> DeleteAsync(int id, int clientId);
}
