using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class RecurringScheduleDtoRequestValidator : AbstractValidator<RecurringScheduleDtoRequest>
{
    public RecurringScheduleDtoRequestValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotEmpty().WithMessage("Dia da semana é obrigatório")
            .IsInEnum().WithMessage("Dia da semana inválido");

        RuleFor(x => x.CloseTime)
            .Must((x, value) => value > x.OpenTime)
                .WithMessage("Horário de encerramento precisa ser superior ao horário de abertura");
    }
}
