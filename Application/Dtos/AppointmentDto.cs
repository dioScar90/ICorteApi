using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

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

    [Required(ErrorMessage = "Data do agendamento obrigatória")]
    [GreaterThanOrEqualToday(ErrorMessage = "Data do agendamento precisa ser igual ou superior a data atual")]
    DateOnly Date,

    [Required(ErrorMessage = "Horário de início obrigatório")]
    TimeOnly StartTime,

    TimeSpan TotalDuration,
    string? Notes,
    PaymentType PaymentType,
    decimal TotalPrice,

    ServiceForUpdateAppointmentDtoRequest[] Services,
    
    AppointmentStatus Status = AppointmentStatus.Pending
) : IDtoRequest<Appointment>;

public record AppointmentPaymentTypeDtoUpdateRequest(
    int ClientId,
    PaymentType PaymentType
) : IDtoRequest<Appointment>;
