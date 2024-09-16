using FluentValidation;

namespace ICorteApi.Application.Validators;

public class AppointmentDtoUpdateValidator : AbstractValidator<AppointmentDtoUpdate>
{
    public AppointmentDtoUpdateValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data do agendamento é obrigatória")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage($"Data do agendamento precisa ser maior ou igual a {DateTime.UtcNow.Date}");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início é obrigatório");
    }
}
