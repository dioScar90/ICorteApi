using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class SpecialScheduleDtoValidator : AbstractValidator<SpecialScheduleDto>
{
    public SpecialScheduleDtoValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Dia obrigatório");

        RuleFor(x => x.CloseTime)
            .Must((x, closeTime) => x.OpenTime is null || closeTime is null || x.OpenTime > closeTime)
                .WithMessage("Horário de encerramento precisa ser superior ao horário de abertura quando os dois forem informados");

        RuleFor(x => x.Notes)
            .MinimumLength(3).WithMessage("Observação, caso informado, precisa ter pelo menos 3 caracteres");
    }
}
