namespace ICorteApi.Application.Dtos;

public record AppointmentDtoCreate(
    DateOnly Date,
    TimeOnly StartTime,
    string? Notes,
    PaymentType PaymentType,
    int[] ServiceIds
) : IDtoRequest<Appointment>;

public record AppointmentDtoUpdate(
    DateOnly Date,
    TimeOnly StartTime,
    string? Notes,
    PaymentType PaymentType,
    int[] ServiceIds
) : IDtoRequest<Appointment>;

public record AppointmentPaymentTypeDtoUpdate(
    PaymentType PaymentType
) : IDtoRequest<Appointment>;

public record AppointmentDtoResponse(
    int Id,
    int ClientId,
    int BarberShopId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeSpan TotalDuration,
    string? Notes,
    PaymentType PaymentType,
    decimal TotalPrice,
    ServiceDtoResponse[] Services,
    AppointmentStatus Status
) : IDtoResponse<Appointment>;
