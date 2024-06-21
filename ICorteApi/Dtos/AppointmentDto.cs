namespace BarberAppApi.Dtos;

public record AppointmentDto(
    DateTime AppointmentDate,
    string ServicesRequested
);
