using FluentValidation;

namespace ICorteApi.Application.Validators;

public class AppointmentValidator : AbstractValidator<AppointmentDtoRequest>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data do agendamento obrigatória")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Data do agendamento precisa ser maior ou igual à data de hoje");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início obrigatório");
    }
}
