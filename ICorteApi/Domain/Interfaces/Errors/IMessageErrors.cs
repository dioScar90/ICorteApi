namespace ICorteApi.Domain.Interfaces;

public interface IMessageErrors : IBaseErrors<Message>
{
    void ThrowMessageNotBelongsToAppointmentException(int appointmentId);
    void ThrowMessageNotBelongsToSenderException(int senderId);
}
