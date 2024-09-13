using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class BarberShopErrors : BaseErrors<BarberShop>, IBarberShopErrors
{
    public void ThrowBarberShopNotBelongsToOwnerException(int ownerId)
    {
        string message = $"{_entity} não pertence ao proprietário \"{ownerId}\" informado";
        throw new ConflictException(message);
    }
}
