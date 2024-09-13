namespace ICorteApi.Domain.Interfaces;

public interface ISpecialScheduleErrors : IBaseErrors<SpecialSchedule>
{
    void ThrowSpecialScheduleNotBelongsToBarberShopException(int barberShopId);
}
