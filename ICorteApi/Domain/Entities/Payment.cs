using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Payment : BasePrimaryKeyEntity<Payment, int>
{
    public PaymentType PaymentType { get; private set; }
    public decimal Amount { get; private set; }
    
    public int AppointmentId { get; init; }
    public Appointment Appointment { get; init; }

    private Payment() {}

    public Payment(PaymentDtoRequest dto, int? appointmentId = null)
    {
        PaymentType = dto.PaymentType;
        Amount = dto.Amount;

        AppointmentId = appointmentId ?? default;
    }

    private void UpdateByPaymentDto(PaymentDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        PaymentType = dto.PaymentType;
        Amount = dto.Amount;
        
        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Payment> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is PaymentDtoRequest dto)
            UpdateByPaymentDto(dto, utcNow);
            
        throw new Exception("Dados enviados inválidos");
    }
}
