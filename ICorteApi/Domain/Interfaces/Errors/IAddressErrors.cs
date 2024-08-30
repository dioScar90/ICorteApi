using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IAddressErrors : IBaseErrors<Address>
{
    void ThrowAddressNotBelongsToBarberShopException(int barberShopId);
}
