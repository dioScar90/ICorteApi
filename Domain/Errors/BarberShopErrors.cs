using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class BarberShopErrors : BaseErrors<BarberShop>
{
    public Conflict<Error> BarberShopBelongsToAnotherOwner()
    {
        string message = $"{_entity} pertence a outro proprietário";
        return Error.Conflict(message);
    }
}
