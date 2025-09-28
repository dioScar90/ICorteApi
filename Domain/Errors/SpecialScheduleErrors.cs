namespace ICorteApi.Domain.Errors;

public sealed class SpecialScheduleErrors : BaseErrors<SpecialSchedule>
{
    public void ThrowSpecialScheduleNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
