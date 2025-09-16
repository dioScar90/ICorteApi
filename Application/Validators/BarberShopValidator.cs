using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class BarberShopDtoValidator : AbstractValidator<BarberShopDto>
{
    public BarberShopDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Description)
            .MinimumLength(3).WithMessage("Descrição precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.ComercialNumber).ApplyPhoneNumberValidation("Telefone comercial");

        RuleFor(x => x.ComercialEmail)
            .NotEmpty().WithMessage("Email obrigatório")
            .Must(LastCharIsNotADot).WithMessage("Email incompleto ou com formato inválido")
            .EmailAddress().WithMessage("Email com formato inválido");

        RuleFor(x => x.Address)
            .SetValidator(new AddressDtoValidator())
            .When(x => x.Address is not null);
    }

    private bool LastCharIsNotADot(string? email) => email is not null && !email.EndsWith('.');
}
