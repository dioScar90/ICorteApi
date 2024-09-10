using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class ReportDtoUpdateValidator : AbstractValidator<ReportDtoUpdate>
{
    public ReportDtoUpdateValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(3).WithMessage("Título precisa ter pelo menos 3 caracteres");
        
        RuleFor(x => x.Content)
            .MinimumLength(3).WithMessage("Comentário precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Rating)
            .NotNull().WithMessage("Nota não pode estar vazia")
            .ExclusiveBetween(MIN_RATING, MAX_RATING).WithMessage($"Nota precisa estar entre {MIN_RATING} e {MAX_RATING}");
    }

    private const int MIN_RATING = 1;
    private const int MAX_RATING = 5;
}
