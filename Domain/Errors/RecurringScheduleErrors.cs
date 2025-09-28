namespace ICorteApi.Domain.Errors;

public sealed class RecurringScheduleErrors : BaseErrors<RecurringSchedule>
{
    public void ThrowRecurringScheduleNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
