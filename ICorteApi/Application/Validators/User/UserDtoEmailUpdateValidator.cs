using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoEmailUpdateValidator : AbstractValidator<UserDtoEmailUpdate>
{
    public UserDtoEmailUpdateValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(dx => dx.Email)
                    .EmailAddress().WithMessage("O email não é válido");
            });
    }
}
