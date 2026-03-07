using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Errors;

public sealed class AdminErrors
{
    private static Error[] GetIdentityErrorIntoBasicError(IdentityError[] identityErrors)
        => [..identityErrors.Select(err => new Error(err.Code, err.Description))];

    public Conflict<Error> NotEqualEmail(params Error[] errors)
    {
        return Error.Conflict("Esse email sequer existe, vai procurar o que fazer", errors);
    }

    public Conflict<Error> NotEqualPassphase(params Error[] errors)
    {
        return Error.Conflict("Frase totalmente diferente do que combinamos", errors);
    }

    public Conflict<Error> NullEmail(params Error[] errors)
    {
        return Error.Conflict("Sem email, sem chance", errors);
    }

    public Conflict<Error> NullPassphase(params Error[] errors)
    {
        return Error.Conflict("Sem frase, sem chance", errors);
    }

    public BadRequest<Error> ThereIsNobodyToBeDeleted(params Error[] errors)
    {
        return Error.BadRequest("Se excluir mais alguém nesse banco vai ficar com length negativo e abrir um buraco negro no espaço-tempo", errors);
    }

    public BadRequest<Error> ThereAreTooManyPeopleHere(params Error[] errors)
    {
        return Error.BadRequest("Já tem gente demais aqui", errors);
    }

    public BadRequest<Error> LimitDateIsLessThanStartDate(params Error[] errors)
    {
        return Error.BadRequest("Data limite é menor que a data de início", errors);
    }

    public BadRequest<Error> ThereIsNobodyHereToSetAppointments(params Error[] errors)
    {
        return Error.BadRequest("Quer marcar horários para quem se nem mesmo possui algum cliente cadastrado?", errors);
    }

    public BadRequest<Error> ThereAreTooManyAppointmentsHere(params Error[] errors)
    {
        return Error.BadRequest("Já tem horários demais marcados aqui nesse sistema", errors);
    }
    
    public NotFound<Error> UserDoesNotExist(string email)
    {
        return Error.NotFound($"Isso non Ecziste => {email}");
    }
    
    public Conflict<Error> ResetPassword(string email, params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível resetar a senha do Usuário => {email}";
        return Error.Conflict(message, GetIdentityErrorIntoBasicError(identityErrors));
    }
}
