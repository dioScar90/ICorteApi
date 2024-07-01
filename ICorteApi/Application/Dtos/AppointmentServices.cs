using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record AppointmentServicesDtoRequest(
    int ServiceId
) : IDtoRequest;

public record AppointmentServicesDtoResponse(
    int ServiceId
) : IDtoRequest;
