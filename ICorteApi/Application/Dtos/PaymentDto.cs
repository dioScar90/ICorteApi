namespace ICorteApi.Dtos;

public record PaymentDtoRequest(
    int AppointmentId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod
) : IDtoRequest;

public record PaymentDtoResponse(
    int Id,
    int AppointmentId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod
) : IDtoResponse;
