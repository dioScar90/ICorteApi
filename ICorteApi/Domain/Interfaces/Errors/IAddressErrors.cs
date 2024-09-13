namespace ICorteApi.Domain.Interfaces;

public interface IAddressErrors : IBaseErrors<Address>
{
    void ThrowAddressNotBelongsToBarberShopException(int barberShopId);
}
