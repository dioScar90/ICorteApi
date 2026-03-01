using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class ServiceErrors : BaseErrors<Service>
{
    public Conflict<Error> ServiceNotBelongsToBarberShop(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        return TypedResults.Conflict(new Error ("Conflict Error", message));
    }
    
    public Conflict<Error> ThereAreStillAppointments(DateOnly[] dates)
    {
        string message = $"{_entity} ainda possui alguns agendamentos não concluídos";
        // var erros = dates.Select(date => new Error("AppointmentDate", date.ToString()));
        
        return TypedResults.Conflict(new Error ("Conflict Error", message));
    }
}
