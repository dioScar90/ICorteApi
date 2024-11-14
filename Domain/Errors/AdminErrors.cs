using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Errors;

public sealed class AdminErrors : IAdminErrors
{
    private static Error[] GetIdentityErrorIntoBasicError(IdentityError[] identityErrors)
        => [..identityErrors.Select(err => new Error(err.Code, err.Description))];

    public void ThrowNotEqualEmailException()
    {
        throw new ConflictException("Esse email sequer existe, vai procurar o que fazer");
    }

    public void ThrowNotEqualPassphaseException()
    {
        throw new ConflictException("Frase totalmente diferente do que combinamos");
    }

    public void ThrowNullEmailException()
    {
        throw new ConflictException("Sem email, sem chance");
    }

    public void ThrowNullPassphaseException()
    {
        throw new ConflictException("Sem frase, sem chance");
    }

    public void ThrowThereIsNobodyToBeDeletedException()
    {
        throw new BadRequestException("Se excluir mais alguém nesse banco vai ficar com length negativo e abrir um buraco negro no espaço-tempo");
    }

    public void ThrowThereAreTooManyPeopleHereException()
    {
        throw new BadRequestException("Já tem gente demais aqui");
    }

    public void ThrowThereIsNobodyHereToSetAppointmentsException()
    {
        throw new BadRequestException("Quer marcar horários para quem se nem mesmo possui algum cliente cadastrado?");
    }

    public void ThrowThereAreTooManyAppointmentsHereException()
    {
        throw new BadRequestException("Já tem horários demais marcados nesse app");
    }
    
    public void ThrowUserDoesNotExistException(string email)
    {
        throw new NotFoundException($"Isso non Ecziste => {email}");
    }
    
    public void ThrowResetPasswordException(string email, params IdentityError[] identityErrors)
    {
        string message = $"Não foi possível resetar a senha do Usuário => {email}";
        var errors = GetIdentityErrorIntoBasicError(identityErrors);

        throw new ConflictException(message, [.. errors]);
    }
}
