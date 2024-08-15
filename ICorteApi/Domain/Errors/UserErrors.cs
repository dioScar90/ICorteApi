using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class UserErrors : BaseErrors<User>, IUserErrors
{
    public void ThrowRegisterNotCompletedException()
    {
        throw new ConflictException($"{_entity} com alguns campos ainda pendentes para completar o cadastro");
    }

    public void ThrowUserAlreadyCreatedException()
    {
        throw new ConflictException($"{_entity} já criado");
    }

    public void ThrowWrongUserIdException(int id)
    {
        throw new ConflictException($"{_entity}'s id is different from \"{id}\" provided");
    }
}
