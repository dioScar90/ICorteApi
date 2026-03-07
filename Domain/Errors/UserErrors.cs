using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Errors;

public sealed class UserErrors : BaseErrors<User>
{
    private static Error[] GetIdentityErrorIntoBasicError(IdentityError[] identityErrors)
        => [..identityErrors.Select(err => new Error(err.Code, err.Description))];
    
    public BadRequest<Error> Create(params IdentityError[] identityErrors)
    {
        return Create(GetIdentityErrorIntoBasicError(identityErrors));
    }

    public BadRequest<Error> Update(params IdentityError[] identityErrors)
    {
        return Update(GetIdentityErrorIntoBasicError(identityErrors));
    }
    
    public BadRequest<Error> Delete(params IdentityError[] identityErrors)
    {
        return Delete(GetIdentityErrorIntoBasicError(identityErrors));
    }
    
    public UnprocessableEntity<Error> AddUserRole(params IdentityError[] identityErrors)
    {
        return Error.UnprocessableEntity("Não foi possível atualizar as permissões", GetIdentityErrorIntoBasicError(identityErrors));
    }
    
    public UnprocessableEntity<Error> RemoveUserRole(params IdentityError[] identityErrors)
    {
        return Error.UnprocessableEntity("Não foi possível atualizar as permissões", GetIdentityErrorIntoBasicError(identityErrors));
    }

    public Conflict<Error> RegisterNotCompleted()
    {
        return Error.Conflict($"{_entity} com alguns campos ainda pendentes para completar o cadastro");
    }

    public Conflict<Error> UserAlreadyCreated()
    {
        return Error.Conflict($"{_entity} já criado");
    }

    public ProblemHttpResult WrongUserId(int id)
    {
        return Error.Forbidden($"Id \"{id}\" informado não pertence ao {_entity}");
    }

    public UnprocessableEntity<Error> BasicUser(params IdentityError[] identityErrors)
    {
        string message = $"Algo errado aconteceu ao tentar atualizar o {_entity}";
        return Error.UnprocessableEntity(message, GetIdentityErrorIntoBasicError(identityErrors));
    }

    public UnprocessableEntity<Error> UpdateEmail(params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível atualizar o email do {_entity}";
        return Error.UnprocessableEntity(message, GetIdentityErrorIntoBasicError(identityErrors));
    }

    public UnprocessableEntity<Error> UpdatePassword(params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível atualizar a senha do {_entity}";
        return Error.UnprocessableEntity(message, GetIdentityErrorIntoBasicError(identityErrors));
    }

    public UnprocessableEntity<Error> UpdatePhoneNumber(params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível atualizar o número de telefone do {_entity}";
        return Error.UnprocessableEntity(message, GetIdentityErrorIntoBasicError(identityErrors));
    }
}
