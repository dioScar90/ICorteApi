using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class ProfileErrors : BaseErrors<Profile>, IProfileErrors
{
    public void ThrowProfileNotBelongsToUserException(int userId)
    {
        string message = $"{_entity} não pertence ao usuário \"{userId}\" informado";
        throw new ConflictException(message);
    }
}
