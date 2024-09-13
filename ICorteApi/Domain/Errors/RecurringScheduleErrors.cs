using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class RecurringScheduleErrors : BaseErrors<RecurringSchedule>, IRecurringScheduleErrors
{
    public void ThrowRecurringScheduleNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
