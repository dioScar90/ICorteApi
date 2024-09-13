using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class ProfileErrors : BaseErrors<Profile>, IProfileErrors
{
    public void ThrowProfileNotBelongsToUserException(int userId)
    {
        string message = $"{_entity} não pertence ao usuário \"{userId}\" informado";
        throw new ConflictException(message);
    }
}
