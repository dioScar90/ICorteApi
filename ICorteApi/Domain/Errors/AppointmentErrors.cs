using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class AppointmentErrors : BaseErrors<Appointment>, IAppointmentErrors
{
    public void ThrowEmptyServicesException()
    {
        throw new UnprocessableEntity("Selecione pelo menos um serviço");
    }

    public void ThrowNotBarberShopIdsUniqueFromServicesException()
    {
        throw new UnprocessableEntity("Serviços escolhidos precisam todos pertencer à mesma barbearia");
    }

    public void ThrowAppointmentNotBelongsToClientException(int clientId)
    {
        string message = $"{_entity} não pertence ao perfil \"{clientId}\" informado";
        throw new ConflictException(message);
    }
}
