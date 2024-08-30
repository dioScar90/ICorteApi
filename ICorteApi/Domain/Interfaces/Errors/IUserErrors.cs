using ICorteApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Interfaces;

public interface IUserErrors : IBaseErrors<User>
{
    void ThrowCreateException(params IdentityError[] identityErrors);
    void ThrowUpdateException(params IdentityError[] identityErrors);
    void ThrowDeleteException(params IdentityError[] identityErrors);
    
    void ThrowRegisterNotCompletedException();
    void ThrowUserAlreadyCreatedException();
    void ThrowWrongUserIdException(int id);

    void ThrowBasicUserException(params IdentityError[] identityErrors);
    void ThrowUpdateEmailException(params IdentityError[] identityErrors);
    void ThrowUpdatePasswordException(params IdentityError[] identityErrors);
    void ThrowUpdatePhoneNumberException(params IdentityError[] identityErrors);
}
