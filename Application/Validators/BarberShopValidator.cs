using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class BarberShopDtoCreateValidator : AbstractValidator<BarberShopDtoCreate>
{
    public BarberShopDtoCreateValidator()
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
            .SetValidator(new AddressDtoCreateValidator())
            .When(x => x.Address is not null);
    }

    private bool LastCharIsNotADot(string? email) => email is not null && !email.EndsWith('.');
}

public sealed class BarberShopDtoUpdateValidator : AbstractValidator<BarberShopDtoUpdate>
{
    public BarberShopDtoUpdateValidator()
    {
        RuleFor(x => new BarberShopDtoCreate(x.Name, x.Description, x.ComercialNumber, x.ComercialEmail, null, null, null, null))
            .SetValidator(new BarberShopDtoCreateValidator());
    }
}
