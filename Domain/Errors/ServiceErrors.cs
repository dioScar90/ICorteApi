using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class ServiceErrors : BaseErrors<Service>
{
    public Conflict<Error> ServiceBelongsToAnotherBarberShop()
    {
        string message = $"{_entity} pertence a outra barbearia";
        return Error.Conflict(message);
    }
    
    public Conflict<Error> ThereAreStillAppointments(DateOnly[] dates)
    {
        string message = $"{_entity} ainda possui alguns agendamentos não concluídos";
        
        if (dates.Length > 0)
        {
            string datesIntoString = string.Join(", ", dates.Select(d => d.ToString("dd/MM/yyyy")));
            message += $", com datas: {datesIntoString}";
        }
        
        return Error.Conflict(message);
    }
}
