using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class PaymentDtoCreateValidator : AbstractValidator<PaymentDtoCreate>
{
    public PaymentDtoCreateValidator()
    {
        RuleFor(x => x.PaymentType)
            .NotEmpty().WithMessage("Tipo de pagamento é obrigatório")
            .IsInEnum().WithMessage("Tipo de pagamento inválido");

        RuleFor(x => x.Amount)
            .NotNull().WithMessage("Valor não pode estar vazio")
            .GreaterThan(0).WithMessage("Valor precisa ser maior que R$ 0,00");
    }
}
