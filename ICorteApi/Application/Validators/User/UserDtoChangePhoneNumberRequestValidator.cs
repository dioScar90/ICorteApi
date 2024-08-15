using System.Text.RegularExpressions;
using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoChangePhoneNumberRequestValidator : AbstractValidator<UserDtoChangePhoneNumberRequest>
{
    public UserDtoChangePhoneNumberRequestValidator()
    {
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é obrigatório")
            .Length(11).WithMessage("Número de telefone precisa ter pelo menos 3 caracteres")
            .Must(IsValidPhoneNumber).WithMessage("Número de telefone precisa estar no formato (xx) 9xxxx-xxxx");
    }
    
    private bool IsValidPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^\d{2}9\d{8}$");
}
