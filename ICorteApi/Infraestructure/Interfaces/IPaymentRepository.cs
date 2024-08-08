using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IPaymentRepository
    : IBasePrimaryKeyRepository<Payment, int>
{
}
