using FluentValidation;

namespace ICorteApi.Application.Validators;

public class AppointmentDtoCreateValidator : AbstractValidator<AppointmentDtoCreate>
{
    public AppointmentDtoCreateValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data do agendamento obrigatória")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Data do agendamento precisa ser maior ou igual à data de hoje");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início obrigatório");
    }
}

public class AppointmentDtoUpdateValidator : AbstractValidator<AppointmentDtoUpdate>
{
    public AppointmentDtoUpdateValidator()
    {
        RuleFor(x => new AppointmentDtoCreate(x.Date, x.StartTime, x.Notes, x.PaymentType, x.ServiceIds))
            .SetValidator(new AppointmentDtoCreateValidator());
    }
}
