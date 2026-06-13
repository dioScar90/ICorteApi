using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class SpecialScheduleErrors : BaseErrors<SpecialSchedule>
{
    public Conflict<Error> SpecialScheduleBelongsToAnotherBarberShop()
    {
        string message = $"{_entity} pertence a outra barbearia";
        return Error.Conflict(message);
    }
}
