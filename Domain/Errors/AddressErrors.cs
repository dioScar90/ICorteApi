namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>
{
    public void ThrowAddressNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
