using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>, IAddressErrors
{
    public void ThrowAddressNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
