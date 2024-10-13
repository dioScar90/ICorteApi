namespace ICorteApi.Domain.Interfaces;

public interface IMessageErrors : IBaseErrors<Message>
{
    void ThrowNotAllowedToSendMessageException(int senderId);
    void ThrowMessageNotBelongsToAppointmentException(int appointmentId);
    void ThrowMessageNotBelongsToSenderException(int senderId);
}
