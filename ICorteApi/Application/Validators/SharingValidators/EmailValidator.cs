using FluentValidation;

namespace ICorteApi.Application.Validators;

internal static class EmailValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyEmailValidation<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email obrigatório")
            .DependentRules(() =>
            {
                ruleBuilder
                    .EmailAddress().WithMessage("Formato de email inválido");
            });
    }
}
