using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record AppointmentServicesDtoRequest(
    int AppointmentId,
    int ServiceId
) : IDtoRequest;

public record AppointmentServicesDtoResponse(
    int AppointmentId,
    int ServiceId
) : IDtoRequest;
