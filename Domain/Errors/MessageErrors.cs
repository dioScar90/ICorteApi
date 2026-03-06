using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class MessageErrors : BaseErrors<Message>
{
    public ProblemHttpResult NotAllowedToSendMessage()
    {
        string message = $"Perfil não autorizado para enviar a mensagem";
        // return TypedResults.Forbid(new Error("Forbid Error", message));
        // return TypedResults.Forbid();
        return TypedResults.Problem(message, statusCode: StatusCodes.Status403Forbidden);
    }
    
    public Conflict<Error> MessageNotBelongsToAppointment()
    {
        string message = $"{_entity} não pertence ao agendamento informado";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
    
    public Conflict<Error> MessageNotBelongsToSender()
    {
        string message = $"{_entity} não pertence ao remetente";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
}
