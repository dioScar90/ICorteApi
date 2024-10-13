using System.Text.RegularExpressions;
using FluentValidation;

namespace ICorteApi.Application.Validators;

internal static class PhoneNumberValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyPhoneNumberValidation<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string? messageIdentifier = null)
    {
        string NumTelefone = messageIdentifier ?? "Número de telefone";

        return ruleBuilder
            .NotEmpty().WithMessage($"{NumTelefone} obrigatório")
            .DependentRules(() =>
            {
                ruleBuilder
                    .Must(IsValidPhoneNumber).WithMessage($"{NumTelefone} precisa estar no formato (xx) 9xxxx-xxxx");
            });
    }

    private static bool IsNull(string? value) => value is null;
    private static bool IsValidPhoneNumber(string? value) => !IsNull(value) && Regex.IsMatch(value!, @"^\d{2}9\d{8}$");
}
