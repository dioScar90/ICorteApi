using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class RecurringScheduleErrors : BaseErrors<RecurringSchedule>
{
    public Conflict<Error> RecurringScheduleBelongsToAnotherBarberShop()
    {
        string message = $"{_entity} não pertence à barbearia informada";
        return Error.Conflict(message);
    }
}
