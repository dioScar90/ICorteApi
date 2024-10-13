using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class AddressDtoCreateValidator : AbstractValidator<AddressDtoCreate>
{
    public AddressDtoCreateValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Logradouro obrigatório")
            .MinimumLength(3).WithMessage("Logradouro precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Número obrigatório");

        RuleFor(x => x.Complement)
            .MinimumLength(3).WithMessage("Complemento precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("Bairro obrigatório")
            .MinimumLength(3).WithMessage("Bairro precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cidade obrigatória")
            .MinimumLength(3).WithMessage("Cidade precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.State)
            .NotNull().WithMessage("Estado obrigatório");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("CEP obrigatório")
            .Length(8).WithMessage("CEP precisa ter 8 dígitos");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("País obrigatório")
            .MinimumLength(3).WithMessage("País precisa ter pelo menos 3 caracteres");
    }
}

public sealed class AddressDtoUpdateValidator : AbstractValidator<AddressDtoUpdate>
{
    public AddressDtoUpdateValidator()
    {
        RuleFor(x => new AddressDtoCreate(x.Street, x.Number, x.Complement, x.Neighborhood, x.City, x.State, x.PostalCode, x.Country))
            .SetValidator(new AddressDtoCreateValidator());
    }
}
