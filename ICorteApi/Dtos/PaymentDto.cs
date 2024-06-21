namespace BarberAppApi.Dtos;

public record PaymentDto(
    int Id,
    int AppointmentId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod
);
