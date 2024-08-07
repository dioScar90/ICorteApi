using System.Text.RegularExpressions;
using FluentValidation;
using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Validators;

public class UserDtoRequestValidator : AbstractValidator<UserDtoRequest>
{
    public UserDtoRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("O e-mail não é válido");

        // RuleFor(x => x.Password)
        //     .NotEmpty().WithMessage("A senha é obrigatória")
        //     .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres")
        //     .Must(ContainUppercase).WithMessage("A senha deve conter pelo menos uma letra maiúscula")
        //     .Must(ContainLowercase).WithMessage("A senha deve conter pelo menos uma letra minúscula")
        //     .Must(ContainDigit).WithMessage("A senha deve conter pelo menos um dígito")
        //     .Must(ContainNonAlphanumeric).WithMessage("A senha deve conter pelo menos um caractere especial");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é obrigatório")
            .Length(11).WithMessage("Número de telefone precisa ter pelo menos 3 caracteres")
            .Must(IsValidPhoneNumber).WithMessage("Número de telefone precisa estar no formato (xx) 9xxxx-xxxx");
    }

    // private bool ContainUppercase(string password) => password.Any(char.IsUpper);
    // private bool ContainLowercase(string password) => password.Any(char.IsLower);
    // private bool ContainDigit(string password) => password.Any(char.IsDigit);
    // private bool ContainNonAlphanumeric(string password) => password.Any(ch => !char.IsLetterOrDigit(ch));
    private bool IsValidPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^\d{2}9\d{8}$");
}
