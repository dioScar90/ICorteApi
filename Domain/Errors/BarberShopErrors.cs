using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class BarberShopErrors : BaseErrors<BarberShop>
{
    public Conflict<Error> BarberShopNotBelongsToOwner()
    {
        string message = $"{_entity} não pertence ao proprietário informado";
        return Error.Conflict(message);
    }
}
