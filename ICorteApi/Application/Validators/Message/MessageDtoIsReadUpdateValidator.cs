using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class MessageDtoIsReadUpdateValidator : AbstractValidator<MessageDtoIsReadUpdate>
{
    public MessageDtoIsReadUpdateValidator()
    {
    }
}
