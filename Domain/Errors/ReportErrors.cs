using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class ReportErrors : BaseErrors<Report>
{
    public Conflict<Error> ReportNotBelongsToClient()
    {
        string message = $"{_entity} não pertence ao cliente";
        return Error.Conflict(message);
    }
    
    public Conflict<Error> ReportNotBelongsToBarberShop(params Error[] errors)
    {
        string message = $"{_entity} não pertence à barbearia informada";
        return Error.Conflict(message);
    }
}
