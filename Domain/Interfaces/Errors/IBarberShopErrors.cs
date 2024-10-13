namespace ICorteApi.Domain.Interfaces;

public interface IBarberShopErrors : IBaseErrors<BarberShop>
{
    void ThrowBarberShopNotBelongsToOwnerException(int ownerId);
}
