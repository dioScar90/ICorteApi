using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class ServiceErrors : BaseErrors<Service>, IServiceErrors
{
    public void ThrowServiceNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }

    public void ThrowThereAreStillAppointmentsException(DateOnly[] dates)
    {
        string message = $"{_entity} ainda possui alguns agendamentos não concluídos";
        var erros = dates.Select(date => new Error("AppointmentDate", date.ToString()));
        
        throw new ConflictException(message, [..erros]);
    }
}
