using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Dtos;

public record AppointmentDtoRequest(
    DateOnly Date,
    TimeOnly StartTime,
    string? Notes,
    int[] ServiceIds
) : IDtoRequest<Appointment>;

public record AppointmentDtoResponse(
    int Id,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Notes,
    decimal TotalPrice,
    ServiceDtoResponse[] Services,
    AppointmentStatus Status
) : IDtoResponse<Appointment>;
