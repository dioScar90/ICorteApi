namespace ICorteApi.Domain.Interfaces;

public interface IServiceErrors : IBaseErrors<Service>
{
    void ThrowServiceNotBelongsToBarberShopException(int barberShopId);
    void ThrowThereAreStillAppointmentsException(DateOnly[] dates);
}
