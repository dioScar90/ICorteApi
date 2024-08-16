using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record ServiceAppointmentDtoRequest(
    int AppointmentId,
    int ServiceId
) : IDtoRequest<ServiceAppointment>;

public record ServiceAppointmentDtoResponse(
    int AppointmentId,
    int ServiceId
) : IDtoResponse<ServiceAppointment>;
