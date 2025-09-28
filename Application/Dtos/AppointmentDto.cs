using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

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

public record AppointmentDtoRequest(
    int ClientId,
    int BarberShopId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeSpan TotalDuration,
    string? Notes,
    PaymentType PaymentType,
    decimal TotalPrice,
    ServiceDtoRequest[] Services,
    AppointmentStatus Status
) : IDtoRequest<Appointment>;

public record AppointmentPaymentTypeDtoUpdateRequest(
    int ClientId,
    PaymentType PaymentType
) : IDtoRequest<Appointment>;
