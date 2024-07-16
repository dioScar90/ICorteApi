using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class AddressDtoRequestValidator : AbstractValidator<AddressDtoRequest>
{
    public AddressDtoRequestValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Logradouro é obrigatório")
            .MinimumLength(3).WithMessage("Logradouro precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Número é obrigatório");

        RuleFor(x => x.Complement)
            .MinimumLength(3).WithMessage("Complemento precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("Bairro é obrigatório")
            .MinimumLength(3).WithMessage("Bairro precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cidade é obrigatória")
            .MinimumLength(3).WithMessage("Cidade precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.State)
            .NotNull().WithMessage("Estado é obrigatório");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("CEP é obrigatório")
            .Length(8).WithMessage("CEP precisa ter 8 dígitos");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("País é obrigatório")
            .MinimumLength(3).WithMessage("País precisa ter pelo menos 3 caracteres");
    }
}
