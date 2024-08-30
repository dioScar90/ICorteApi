using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IProfileErrors : IBaseErrors<Profile>
{
    void ThrowProfileNotBelongsToUserException(int userId);
}
