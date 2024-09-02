using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Errors;

public sealed class UserErrors : BaseErrors<User>, IUserErrors
{
    private static Error[] GetIdentityErrorIntoBasicError(IdentityError[] identityErrors)
        => [..identityErrors.Select(err => new Error(err.Code, err.Description))];
    
    public void ThrowCreateException(params IdentityError[] identityErrors)
    {
        var errors = GetIdentityErrorIntoBasicError(identityErrors);
        ThrowCreateException([..errors]);
    }

    public void ThrowUpdateException(params IdentityError[] identityErrors)
    {
        var errors = GetIdentityErrorIntoBasicError(identityErrors);
        ThrowUpdateException([..errors]);
    }
    
    public void ThrowDeleteException(params IdentityError[] identityErrors)
    {
        var errors = GetIdentityErrorIntoBasicError(identityErrors);
        ThrowDeleteException([..errors]);
    }
    
    public void ThrowAddUserRoleException(params IdentityError[] identityErrors)
    {
        var errors = GetIdentityErrorIntoBasicError(identityErrors);
        ThrowBadRequestException([..errors]);
    }
    
    public void ThrowRemoveUserRoleException(params IdentityError[] identityErrors)
    {
        var errors = GetIdentityErrorIntoBasicError(identityErrors);
        ThrowBadRequestException([..errors]);
    }

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
        throw new ConflictException($"Id \"{id}\" informado não pertence ao {_entity}");
    }

    public void ThrowBasicUserException(params IdentityError[] identityErrors)
    {
        string message = $"Algo errado aconteceu ao tentar atualizar o {_entity}";
        var errors = GetIdentityErrorIntoBasicError(identityErrors);

        throw new ConflictException(message, [.. errors]);
    }

    public void ThrowUpdateEmailException(params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível atualizar o email do {_entity}";
        var errors = GetIdentityErrorIntoBasicError(identityErrors);

        throw new ConflictException(message, [.. errors]);
    }

    public void ThrowUpdatePasswordException(params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível atualizar a senha do {_entity}";
        var errors = GetIdentityErrorIntoBasicError(identityErrors);

        throw new ConflictException(message, [.. errors]);
    }

    public void ThrowUpdatePhoneNumberException(params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível atualizar o número de telefone do {_entity}";
        var errors = GetIdentityErrorIntoBasicError(identityErrors);

        throw new ConflictException(message, [.. errors]);
    }
}
