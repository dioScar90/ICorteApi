using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class RecurringScheduleErrors : BaseErrors<RecurringSchedule>
{
    public Conflict<Error> RecurringScheduleBelongsToAnotherDayOfWeek()
    {
        string message = $"{_entity} não é do dia da semana informado";
        return Error.Conflict(message);
    }

    public Conflict<Error> RecurringScheduleBelongsToAnotherBarberShop()
    {
        string message = $"{_entity} não pertence à barbearia informado";
        return Error.Conflict(message);
    }
}
