using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class SpecialScheduleDtoCreateValidator : AbstractValidator<SpecialScheduleDtoCreate>
{
    public SpecialScheduleDtoCreateValidator()
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

public sealed class SpecialScheduleDtoUpdateValidator : AbstractValidator<SpecialScheduleDtoUpdate>
{
    public SpecialScheduleDtoUpdateValidator()
    {
        RuleFor(x => new SpecialScheduleDtoCreate(x.Date, x.Notes, x.OpenTime, x.CloseTime, x.IsClosed))
            .SetValidator(new SpecialScheduleDtoCreateValidator());
    }
}
