using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class PaymentErrors : BaseErrors<Payment>, IPaymentErrors
{
    public void ThrowPaymentNotBelongsToAppointmentException(int appointmentId)
    {
        string message = $"{_entity} não pertence ao agendamento \"{appointmentId}\" informado";
        throw new ConflictException(message);
    }
}
