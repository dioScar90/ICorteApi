using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class ServiceDtoValidator : AbstractValidator<ServiceDto>
{
    public ServiceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição obrigatória")
            .MinimumLength(3).WithMessage("Descrição precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Preço obrigatório")
            .GreaterThan(0).WithMessage("Preço precisa ser maior que R$ 0,00");

        RuleFor(x => x.Duration)
            .NotNull().WithMessage("Duração do serivço não pode estar vazia");
    }
}
