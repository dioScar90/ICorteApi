using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class UserDtoPasswordUpdateValidator : AbstractValidator<UserDtoPasswordUpdateRequest>
{
    public UserDtoPasswordUpdateValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Senha atual obrigatória");
            
        RuleFor(x => x.NewPassword).ApplyPasswordValidation("Nova senha");
    }
}

public sealed class UserDtoEmailUpdateValidator : AbstractValidator<UserDtoEmailUpdate>
{
    public UserDtoEmailUpdateValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailValidation();
    }
}

public sealed class UserDtoPhoneNumberUpdateValidator : AbstractValidator<UserDtoPhoneNumberUpdate>
{
    public UserDtoPhoneNumberUpdateValidator()
    {
        RuleFor(x => x.PhoneNumber).ApplyPhoneNumberValidation();
    }
}

public sealed class UserValidator : AbstractValidator<UserDtoRegisterRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).ApplyEmailValidation();
        
        RuleFor(x => x.Password).ApplyPasswordValidation();

        RuleFor(x => x.Profile)
            .SetValidator(new ProfileValidator()!)
            .When(x => x.Profile is not null);
    }
}

public sealed class UserDtoLoginRequestValidator : AbstractValidator<UserDtoLoginRequest>
{
    public UserDtoLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email obrigatório");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha obrigatória");
    }
}
