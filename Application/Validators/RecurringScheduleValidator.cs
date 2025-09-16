using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class RecurringScheduleDtoValidator : AbstractValidator<RecurringScheduleDto>
{
    public RecurringScheduleDtoValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotEmpty().WithMessage("Dia da semana obrigatório")
            .IsInEnum().WithMessage("Dia da semana inválido");

        RuleFor(x => x.CloseTime)
            .Must((x, value) => value > x.OpenTime)
                .WithMessage("Horário de encerramento precisa ser superior ao horário de abertura");
    }
}
