using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class ScheduleDtoRequestValidator : AbstractValidator<ScheduleDtoRequest>
{
    public ScheduleDtoRequestValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotEmpty().WithMessage("Dia da semana é obrigatório")
            .IsInEnum().WithMessage("Dia da semana inválido");

        RuleFor(x => x.EndTime)
            .Must((x, value) => value > x.StartTime)
                .WithMessage("Horário de encerramento precisa ser superior ao horário de abertura");
    }
}
