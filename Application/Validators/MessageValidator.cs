using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class MessageDtoValidator : AbstractValidator<MessageDto>
{
    public MessageDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Mensagem não pode estar vazia")
            .MaximumLength(255).WithMessage("Mensagem não pode ser maior que 255 caracteres");
    }
}
