using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed class ProfileErrors : BaseErrors<Profile>
{
    public Conflict<Error> ProfileNotBelongsToUser()
    {
        string message = $"{_entity} não pertence ao usuário informado";
        return TypedResults.Conflict(new Error("Conflict Error", message));
    }
}
