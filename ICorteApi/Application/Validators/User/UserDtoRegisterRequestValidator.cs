using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoRegisterRequestValidator : AbstractValidator<UserDtoRegisterRequest>
{
    public UserDtoRegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("O e-mail não é válido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres")
            .Must(ContainUppercase).WithMessage("A senha deve conter pelo menos uma letra maiúscula")
            .Must(ContainLowercase).WithMessage("A senha deve conter pelo menos uma letra minúscula")
            .Must(ContainDigit).WithMessage("A senha deve conter pelo menos um dígito")
            .Must(ContainNonAlphanumeric).WithMessage("A senha deve conter pelo menos um caractere especial");
    }

    private bool ContainUppercase(string password) => password.Any(char.IsUpper);
    private bool ContainLowercase(string password) => password.Any(char.IsLower);
    private bool ContainDigit(string password) => password.Any(char.IsDigit);
    private bool ContainNonAlphanumeric(string password) => password.Any(ch => !char.IsLetterOrDigit(ch));
}
