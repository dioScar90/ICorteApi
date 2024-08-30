using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class RecurringScheduleErrors : BaseErrors<RecurringSchedule>, IRecurringScheduleErrors
{
    public void ThrowRecurringScheduleNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
