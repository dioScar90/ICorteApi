namespace ICorteApi.Domain.Interfaces;

public interface IAppointmentErrors : IBaseErrors<Appointment>
{
    void ThrowEmptyServicesException();
    void ThrowNotBarberShopIdsUniqueFromServicesException();
    void ThrowAppointmentNotBelongsToClientException(int clientId);
}
