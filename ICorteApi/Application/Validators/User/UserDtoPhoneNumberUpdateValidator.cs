using System.Text.RegularExpressions;
using FluentValidation;

namespace ICorteApi.Application.Validators;

public class UserDtoPhoneNumberUpdateValidator : AbstractValidator<UserDtoPhoneNumberUpdate>
{
    public UserDtoPhoneNumberUpdateValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é obrigatório")
            .Must(IsValidPhoneNumber).WithMessage("Número de telefone precisa estar no formato (xx) 9xxxx-xxxx");
    }

    private bool IsValidPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^\d{2}9\d{8}$");
}
