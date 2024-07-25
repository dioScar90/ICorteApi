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
                .WithMessage($"Data do agendamento precisa ser maior ou igual a {DateTime.UtcNow}");
    }
}
