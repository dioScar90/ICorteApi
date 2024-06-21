namespace ICorteApi.Dtos;

public record PaymentDto(
    int Id,
    int AppointmentId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod
);
