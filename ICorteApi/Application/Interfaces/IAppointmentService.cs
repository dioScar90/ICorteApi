using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService
    : IBasePrimaryKeyService<Appointment, int>, IHasOneForeignKeyService<Appointment, int>
{
    new Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dtoRequest, int clientId);
}
