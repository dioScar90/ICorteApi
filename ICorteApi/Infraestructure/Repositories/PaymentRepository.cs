using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class PaymentRepository(AppDbContext context)
    : BaseRepository<Payment>(context), IPaymentRepository
{
}
