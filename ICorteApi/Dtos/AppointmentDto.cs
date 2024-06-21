namespace ICorteApi.Dtos;

public record AppointmentDto(
    DateTime AppointmentDate,
    string ServicesRequested
);
