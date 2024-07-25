using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IAppointmentServiceService
    : IBaseCompositeKeyService<AppointmentService, int, int>
{
}
