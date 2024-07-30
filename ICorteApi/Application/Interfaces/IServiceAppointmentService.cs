using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IServiceAppointmentService
    : IBaseCompositeKeyService<ServiceAppointment, int, int>
{
}
