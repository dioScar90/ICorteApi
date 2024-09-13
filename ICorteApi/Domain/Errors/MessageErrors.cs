using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class MessageErrors : BaseErrors<Message>, IMessageErrors
{
    public void ThrowMessageNotBelongsToAppointmentException(int appointmentId)
    {
        string message = $"{_entity} não pertence ao agendamento \"{appointmentId}\" informado";
        throw new ConflictException(message);
    }

    public void ThrowMessageNotBelongsToSenderException(int senderId)
    {
        string message = $"{_entity} não pertence ao remetente \"{senderId}\" informado";
        throw new ConflictException(message);
    }
}
