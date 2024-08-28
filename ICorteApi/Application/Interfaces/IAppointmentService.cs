using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService : IService<Appointment>
{
    Task<ISingleResponse<Appointment>> CreateAsync(AppointmentDtoRequest dtoRequest, int clientId);
    Task<ISingleResponse<Appointment>> GetByIdAsync(int id);
    Task<IResponse> UpdateAsync(AppointmentDtoRequest dtoRequest, int id, int clientId);
    Task<IResponse> DeleteAsync(int id, int clientId);
}
