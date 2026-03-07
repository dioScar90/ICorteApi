using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>
{
    public Conflict<Error> AddressNotBelongsToBarberShop()
    {
        string message = $"{_entity} não pertence à barbearia";
        return Error.Conflict(message);
    }
}
