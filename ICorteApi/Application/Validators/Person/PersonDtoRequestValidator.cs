using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class PersonDtoRequestValidator : AbstractValidator<PersonDtoRequest>
{
    public PersonDtoRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Sobrenome é obrigatório")
            .MinimumLength(3).WithMessage("Sobrenome precisa ter pelo menos 3 caracteres");
    }
}
