using FluentValidation;

namespace ICorteApi.Application.Validators;

public class UserDtoLoginRequestValidator : AbstractValidator<UserDtoLoginRequest>
{
    public UserDtoLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória");
    }
}
