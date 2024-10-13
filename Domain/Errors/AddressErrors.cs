using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>, IAddressErrors
{
    public void ThrowAddressNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
