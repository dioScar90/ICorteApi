using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoChangeEmailRequestValidator : AbstractValidator<UserDtoChangeEmailRequest>
{
    public UserDtoChangeEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("O e-mail não é válido");
    }
}
