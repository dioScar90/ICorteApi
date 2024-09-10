using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoRegisterCreateValidator : AbstractValidator<UserDtoRegisterCreate>
{
    public UserDtoRegisterCreateValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(dx => dx.Email)
                    .EmailAddress().WithMessage("O email não é válido");
            });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .DependentRules(() =>
            {
                RuleFor(dx => dx.Password)
                    .MinimumLength(8).WithMessage("A senha deve conter pelo menos 8 caracteres")
                    .Must(ContainUppercase).WithMessage("A senha deve conter pelo menos uma letra maiúscula")
                    .Must(ContainLowercase).WithMessage("A senha deve conter pelo menos uma letra minúscula")
                    .Must(ContainDigit).WithMessage("A senha deve conter pelo menos um dígito")
                    .Must(ContainNonAlphanumeric).WithMessage("A senha deve conter pelo menos um caractere especial");
            });
    }

    private bool ContainUppercase(string password) => password is not null && password.Any(char.IsUpper);
    private bool ContainLowercase(string password) => password is not null && password.Any(char.IsLower);
    private bool ContainDigit(string password) => password is not null && password.Any(char.IsDigit);
    private bool ContainNonAlphanumeric(string password) => password is not null && password.Any(ch => !char.IsLetterOrDigit(ch));
}
