namespace ICorteApi.Dtos;

public record AppointmentDtoRequest(
    DateTime AppointmentDate,
    AppointmentServicesDtoRequest[]? AppointmentServices
) : IDtoRequest;

public record AppointmentDtoResponse(
    DateTime AppointmentDate,
    AppointmentServicesDtoResponse[]? AppointmentServices
) : IDtoResponse;
