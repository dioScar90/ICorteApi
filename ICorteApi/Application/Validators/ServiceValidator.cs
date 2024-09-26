using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class ServiceDtoCreateValidator : AbstractValidator<ServiceDtoCreate>
{
    public ServiceDtoCreateValidator()
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

public sealed class ServiceDtoUpdateValidator : AbstractValidator<ServiceDtoUpdate>
{
    public ServiceDtoUpdateValidator()
    {
        RuleFor(x => new ServiceDtoCreate(x.Name, x.Description, x.Price, x.Duration))
            .SetValidator(new ServiceDtoCreateValidator());
    }
}
