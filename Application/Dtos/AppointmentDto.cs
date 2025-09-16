namespace ICorteApi.Application.Dtos;

public record AppointmentDto(
    int Id,
    int ClientId,
    int BarberShopId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeSpan TotalDuration,
    string? Notes,
    PaymentType PaymentType,
    decimal TotalPrice,
    ServiceDto[] Services,
    AppointmentStatus Status
) : IDto<Appointment>;

public record AppointmentPaymentTypeDtoUpdate(
    PaymentType PaymentType
) : IDto<Appointment>;
