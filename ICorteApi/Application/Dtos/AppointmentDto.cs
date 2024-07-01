using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record AppointmentDtoRequest(
    DateTime AppointmentDate,
    AppointmentServicesDtoRequest[]? AppointmentServices
) : IDtoRequest;

public record AppointmentDtoResponse(
    DateTime AppointmentDate,
    AppointmentServicesDtoResponse[]? AppointmentServices
) : IDtoResponse;
