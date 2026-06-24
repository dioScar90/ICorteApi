using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class AppointmentErrors : BaseErrors<Appointment>
{
    public UnprocessableEntity<Error> EmptyServices()
    {
        string message = "Selecione pelo menos um serviço";
        return Error.UnprocessableEntity(message);
    }
    
    public UnprocessableEntity<Error> NotBarberShopIdsUniqueFromServices()
    {
        string message = "Serviços escolhidos precisam todos pertencer à mesma barbearia";
        return Error.UnprocessableEntity(message);
    }
    
    public Conflict<Error> AppointmentBelongsToAnotherClient()
    {
        string message = $"{_entity} pertence a outro cliente";
        return Error.Conflict(message);
    }
}
