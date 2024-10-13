using FluentValidation;

namespace ICorteApi.Application.Validators;

internal static class PasswordValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyPasswordValidation<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string? messageIdentifier = null)
    {
        string Senha = messageIdentifier ?? "Senha";
        
        return ruleBuilder
            .NotEmpty().WithMessage($"{Senha} obrigatória")
            .DependentRules(() =>
            {
                ruleBuilder
                    .MinimumLength(8).WithMessage($"{Senha} deve conter pelo menos 8 caracteres")
                    .Must(ContainUppercase).WithMessage($"{Senha} deve conter pelo menos uma letra maiúscula")
                    .Must(ContainLowercase).WithMessage($"{Senha} deve conter pelo menos uma letra minúscula")
                    .Must(ContainDigit).WithMessage($"{Senha} deve conter pelo menos um dígito")
                    .Must(ContainNonAlphanumeric).WithMessage($"{Senha} deve conter pelo menos um caractere especial");
            });
    }

    private static bool IsNull(string? value) => value is null;

    private static bool ContainUppercase(string? password) => !IsNull(password) && password!.Any(char.IsUpper);
    private static bool ContainLowercase(string? password) => !IsNull(password) && password!.Any(char.IsLower);
    private static bool ContainDigit(string? password) => !IsNull(password) && password!.Any(char.IsDigit);
    private static bool ContainNonAlphanumeric(string? password) => !IsNull(password) && password!.Any(ch => !char.IsLetterOrDigit(ch));
}
