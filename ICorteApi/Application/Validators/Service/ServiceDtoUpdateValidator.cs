using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class ServiceDtoUpdateValidator : AbstractValidator<ServiceDtoUpdate>
{
    public ServiceDtoUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Título não pode estar vazio")
            .MinimumLength(3).WithMessage("Título precisa ter pelo menos 3 caracteres");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição não pode estar vazia")
            .MinimumLength(3).WithMessage("Descrição precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Preço do serviço não pode estar vazio")
            .GreaterThan(0).WithMessage("Preço precisa ser maior que R$ 0,00");

        // RuleFor(x => x.Duration)
        //     .NotNull().WithMessage("Duração do serivço não pode estar vazia")
        //     // .Must(TimeSpanChecker).WithMessage("Duração precisa estar no formato \"mm:ss\" ou \"hh:mm:ss\"");
        //     .Must(TimeSpanChecker).WithMessage("Tempo de duração inválido");

        RuleFor(x => x.Duration)
            .NotNull().WithMessage("Duração do serivço não pode estar vazia");
    }
    
    // private static bool TimeSpanChecker(string value) => Regex.IsMatch(value, @"^[\d{2}\:]\d{2}\:\d{2}\:\d{2}$");
    // private static bool TimeSpanChecker(string value) => Regex.IsMatch(value, @"^\d{2}:\d{2}(:\d{2})?$");
}
