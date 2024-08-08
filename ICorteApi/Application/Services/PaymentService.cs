using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class PaymentService(IPaymentRepository repository)
    : BasePrimaryKeyService<Payment, int>(repository), IPaymentService
{
    public async Task<ISingleResponse<Payment>> CreateAsync(IDtoRequest<Payment> dto, int appointmentId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.AppointmentId = appointmentId;

        return await CreateByEntityAsync(entity);
    }
}
