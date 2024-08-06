using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Dtos;

public record AppointmentDtoRequest(
    int BarberShopId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Notes,
    decimal TotalPrice,
    AppointmentStatus Status
    // AppointmentServicesDtoResponse[]? AppointmentServices
) : IDtoRequest<Appointment>;

public record AppointmentDtoResponse(
    int Id,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Notes,
    decimal TotalPrice,
    AppointmentStatus Status
    // AppointmentServicesDtoResponse[]? AppointmentServices
) : IDtoResponse<Appointment>;
