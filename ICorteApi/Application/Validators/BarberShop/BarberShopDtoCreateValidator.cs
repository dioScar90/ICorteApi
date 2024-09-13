using FluentValidation;

namespace ICorteApi.Application.Validators;

public class BarberShopDtoCreateValidator : AbstractValidator<BarberShopDtoCreate>
{
    public BarberShopDtoCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Description)
            .MinimumLength(3).WithMessage("Descrição precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.ComercialNumber)
            
            .MinimumLength(3).WithMessage("Complemento precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.ComercialEmail)
            .NotEmpty().WithMessage("Email é obrigatório")
            .Must(LastCharIsNotADot).WithMessage("Email incompleto ou com formato inválido")
            .EmailAddress().WithMessage("Email com formato inválido");
    }

    private bool LastCharIsNotADot(string email) => !email.EndsWith('.');
}
