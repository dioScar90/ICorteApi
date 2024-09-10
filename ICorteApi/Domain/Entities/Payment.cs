using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Payment : BaseEntity<Payment>
{
    public PaymentType PaymentType { get; private set; }
    public decimal Amount { get; private set; }

    public int AppointmentId { get; init; }
    public Appointment Appointment { get; init; }

    private Payment() { }

    public Payment(PaymentDtoCreate dto, int? appointmentId = null)
    {
        PaymentType = dto.PaymentType;
        Amount = dto.Amount;

        AppointmentId = appointmentId ?? default;
    }

    private void UpdateByPaymentDto(PaymentDtoCreate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        PaymentType = dto.PaymentType;
        Amount = dto.Amount;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Payment> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case PaymentDtoCreate dto:
                UpdateByPaymentDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override PaymentDtoResponse CreateDto() =>
        new(
            Id,
            AppointmentId,
            PaymentType,
            Amount
        );
}
