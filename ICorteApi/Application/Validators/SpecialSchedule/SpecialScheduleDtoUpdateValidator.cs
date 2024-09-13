using FluentValidation;

namespace ICorteApi.Application.Validators;

public class SpecialScheduleDtoUpdateValidator : AbstractValidator<SpecialScheduleDtoUpdate>
{
    public SpecialScheduleDtoUpdateValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Dia é obrigatório");

        // RuleFor(x => new { x.OpenTime, x.CloseTime })
        //     .Must(OpenAndCloseTimesAreNotBothNull)
        //     .When(x => !x.IsClosed)
        //         .WithMessage("Horário de abertura ou horário de encerramento precisam ser informados quando for um dia fechado");
        
        RuleFor(x => x.Notes)
            .MinimumLength(3).WithMessage("Observação, caso informado, precisa ter pelo menos 3 caracteres");
    }
    
    // private static bool OpenAndCloseTimesAreNotBothNull(object x) =>
    //     !(x is ITwoTimes twoTimes && twoTimes is { OpenTime: null, CloseTime: null });

    // private interface ITwoTimes
    // {
    //     TimeSpan? OpenTime { get; }
    //     TimeSpan? CloseTime { get; }
    // }
}
