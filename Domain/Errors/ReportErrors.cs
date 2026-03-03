using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class ReportErrors : BaseErrors<Report>
{
    public Conflict<Error> ReportNotBelongsToClient()
    {
        string message = $"{_entity} não pertence ao cliente";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
    
    public Conflict<Error> ReportNotBelongsToBarberShop()
    {
        string message = $"{_entity} não pertence à barbearia informada";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
}
