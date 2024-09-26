using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class MessageDtoCreateValidator : AbstractValidator<MessageDtoCreate>
{
    public MessageDtoCreateValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Mensagem não pode estar vazia")
            .MaximumLength(255).WithMessage("Mensagem não pode ser maior que 255 caracteres");
    }
}

public sealed class MessageDtoIsReadUpdateValidator : AbstractValidator<MessageDtoIsReadUpdate>
{
    public MessageDtoIsReadUpdateValidator()
    {
    }
}
