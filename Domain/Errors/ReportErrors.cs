using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class ReportErrors : BaseErrors<Report>
{
    public Conflict<Error> ReportBelongsToAnotherClient()
    {
        string message = $"{_entity} pertence a outro cliente";
        return Error.Conflict(message);
    }
    
    public Conflict<Error> ReportBelongsToAnotherBarberShop(params Error[] errors)
    {
        string message = $"{_entity} pertence a outra barbearia";
        return Error.Conflict(message);
    }
}
