using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class SpecialScheduleErrors : BaseErrors<SpecialSchedule>
{
    public Conflict<Error> SpecialScheduleNotBelongsToBarberShop()
    {
        string message = $"{_entity} não pertence à barbearia informada";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
}
