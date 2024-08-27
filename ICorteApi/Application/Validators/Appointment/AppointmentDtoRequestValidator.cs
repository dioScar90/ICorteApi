using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class AppointmentDtoRequestValidator : AbstractValidator<AppointmentDtoRequest>
{
    public AppointmentDtoRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data do agendamento é obrigatória")
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage($"Data do agendamento precisa ser maior ou igual a {DateTime.UtcNow.Date}");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início é obrigatório")
            .GreaterThan(TimeOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage($"Horário de início precisa ser maior ou igual a {DateTime.UtcNow.TimeOfDay}");
    }
}
