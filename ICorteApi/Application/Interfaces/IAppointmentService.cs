using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentService
    : IBasePrimaryKeyService<Appointment, int>, IHasTwoForeignKeyService<Appointment, int, int>
{
    new Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dto, int clientId, int barberShopId);
}
