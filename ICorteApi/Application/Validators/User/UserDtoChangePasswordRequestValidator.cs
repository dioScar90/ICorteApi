using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoChangePasswordRequestValidator : AbstractValidator<UserDtoChangePasswordRequest>
{
    public UserDtoChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("A senha atual é obrigatória");
        
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("A nova senha é obrigatória")
            .DependentRules(() => {
                RuleFor(dx => dx.NewPassword)
                    .MinimumLength(8).WithMessage("A nova senha deve conter pelo menos 8 caracteres")
                    .Must(ContainUppercase).WithMessage("A nova senha deve conter pelo menos uma letra maiúscula")
                    .Must(ContainLowercase).WithMessage("A nova senha deve conter pelo menos uma letra minúscula")
                    .Must(ContainDigit).WithMessage("A nova senha deve conter pelo menos um dígito")
                    .Must(ContainNonAlphanumeric).WithMessage("A nova senha deve conter pelo menos um caractere especial");
            });
    }

    private bool ContainUppercase(string password) => password is not null && password.Any(char.IsUpper);
    private bool ContainLowercase(string password) => password is not null && password.Any(char.IsLower);
    private bool ContainDigit(string password) => password is not null && password.Any(char.IsDigit);
    private bool ContainNonAlphanumeric(string password) => password is not null && password.Any(ch => !char.IsLetterOrDigit(ch));
}
