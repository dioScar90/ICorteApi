using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class RecurringScheduleDtoCreateValidator : AbstractValidator<RecurringScheduleDtoCreate>
{
    public RecurringScheduleDtoCreateValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotEmpty().WithMessage("Dia da semana obrigatório")
            .IsInEnum().WithMessage("Dia da semana inválido");

        RuleFor(x => x.CloseTime)
            .Must((x, value) => value > x.OpenTime)
                .WithMessage("Horário de encerramento precisa ser superior ao horário de abertura");
    }
}

public sealed class RecurringScheduleDtoUpdateValidator : AbstractValidator<RecurringScheduleDtoUpdate>
{
    public RecurringScheduleDtoUpdateValidator()
    {
        RuleFor(x => new RecurringScheduleDtoCreate(x.DayOfWeek, x.OpenTime, x.CloseTime))
            .SetValidator(new RecurringScheduleDtoCreateValidator());
    }
}
