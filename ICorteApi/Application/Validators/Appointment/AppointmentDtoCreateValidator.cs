using FluentValidation;

namespace ICorteApi.Application.Validators;

public class AppointmentDtoCreateValidator : AbstractValidator<AppointmentDtoCreate>
{
    public AppointmentDtoCreateValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data do agendamento é obrigatória")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage($"Data do agendamento precisa ser maior ou igual a {DateOnly.FromDateTime(DateTime.UtcNow)}");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início é obrigatório");
    }
}
