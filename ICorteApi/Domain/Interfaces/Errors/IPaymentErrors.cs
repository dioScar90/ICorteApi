using ICorteApi.Domain.Entities;

namespace ICorteApi.Domain.Interfaces;

public interface IPaymentErrors : IBaseErrors<Payment>
{
    void ThrowPaymentNotBelongsToAppointmentException(int appointmentId);
}
