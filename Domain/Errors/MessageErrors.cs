using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class MessageErrors : BaseErrors<Message>
{
    public ProblemHttpResult NotAllowedToSendMessage()
    {
        string message = $"Perfil não autorizado para enviar a mensagem";
        return Error.Forbidden(message);
    }
    
    public Conflict<Error> MessageNotBelongsToAppointment()
    {
        string message = $"{_entity} não pertence ao agendamento informado";
        return Error.Conflict(message);
    }
    
    public Conflict<Error> MessageNotBelongsToSender()
    {
        string message = $"{_entity} não pertence ao remetente";
        return Error.Conflict(message);
    }
}
