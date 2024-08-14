using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class PaymentService(IPaymentRepository repository)
    : BasePrimaryKeyService<Payment, int>(repository), IPaymentService
{
    public async Task<ISingleResponse<Payment>> CreateAsync(IDtoRequest<Payment> dtoRequest, int appointmentId)
    {
        if (dtoRequest is not PaymentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Payment(dto, appointmentId);
        return await CreateByEntityAsync(entity);
    }
}
