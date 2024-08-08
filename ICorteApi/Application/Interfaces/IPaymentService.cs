using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IPaymentService
    : IBasePrimaryKeyService<Payment, int>, IHasOneForeignKeyService<Payment, int>
{
    new Task<ISingleResponse<Payment>> CreateAsync(IDtoRequest<Payment> dto, int appointmentId);
}
