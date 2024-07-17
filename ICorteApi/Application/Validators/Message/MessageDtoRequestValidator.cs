using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class MessageDtoRequestValidator : AbstractValidator<MessageDtoRequest>
{
    public MessageDtoRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Mensagem é não pode estar vazia")
            .Must(NotNullOrWhiteSpace).WithMessage("Mensagem é não pode estar vazia")
            .MaximumLength(255).WithMessage("Mensagem não pode ser maior que 255 caracteres");
    }

    private bool NotNullOrWhiteSpace(string content) => !string.IsNullOrWhiteSpace(content);
}
