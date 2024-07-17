using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Validators;

public class ReportDtoRequestValidator : AbstractValidator<ReportDtoRequest>
{
    public ReportDtoRequestValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(3).WithMessage("Título precisa ter pelo menos 3 caracteres");
        
        RuleFor(x => x.Content)
            .MinimumLength(3).WithMessage("Comentário precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Rating)
            .NotNull().WithMessage("Nota não pode estar vazia")
            .IsInEnum().WithMessage(GetMessageForNotValidRating());
    }

    private static string GetMessageForNotValidRating()
    {
        int[] values = Enum.GetValues(typeof(Rating)).Cast<int>().ToArray();
        return $"Nota precisa estar entre {values.Min()} e {values.Max()}";
    }
}
