using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>
{
    public Conflict<Error> AddressBelongsToAnotherBarberShop()
    {
        string message = $"{_entity} pertence a outra barbearia";
        return Error.Conflict(message);
    }
}
