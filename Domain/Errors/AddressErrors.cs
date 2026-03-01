using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>
{
    public Conflict<Error> AddressNotBelongsToBarberShop(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
}
