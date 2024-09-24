using FluentValidation;

namespace ICorteApi.Application.Validators;

public class UserDtoRegisterCreateValidator : AbstractValidator<UserDtoRegisterCreate>
{
    public UserDtoRegisterCreateValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email obrigatório")
            .DependentRules(() =>
            {
                RuleFor(dx => dx.Email)
                    .EmailAddress().WithMessage("Formato de email inválido");
            });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha obrigatória")
            .DependentRules(() =>
            {
                RuleFor(dx => dx.Password)
                    .MinimumLength(8).WithMessage("Senha deve conter pelo menos 8 caracteres")
                    .Must(ContainUppercase).WithMessage("Senha deve conter pelo menos uma letra maiúscula")
                    .Must(ContainLowercase).WithMessage("Senha deve conter pelo menos uma letra minúscula")
                    .Must(ContainDigit).WithMessage("Senha deve conter pelo menos um dígito")
                    .Must(ContainNonAlphanumeric).WithMessage("Senha deve conter pelo menos um caractere especial");
            });
    }

    private bool ContainUppercase(string password) => password is not null && password.Any(char.IsUpper);
    private bool ContainLowercase(string password) => password is not null && password.Any(char.IsLower);
    private bool ContainDigit(string password) => password is not null && password.Any(char.IsDigit);
    private bool ContainNonAlphanumeric(string password) => password is not null && password.Any(ch => !char.IsLetterOrDigit(ch));
}
