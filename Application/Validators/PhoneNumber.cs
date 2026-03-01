using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ICorteApi.Application.Validators;

public partial class PhoneNumberAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string phoneNumber)
            return new ValidationResult(ErrorMessage);
        
        if (!IsValidPhoneNumber(phoneNumber))
            return new ValidationResult("Número de telefone precisa estar no formato (xx) 9xxxx-xxxx");

        return ValidationResult.Success!;
    }
    
    private static bool IsValidPhoneNumber(string? value) => value is not null && MyRegex().IsMatch(value);
    
    [GeneratedRegex(@"^\d{2}9\d{8}$")]
    private static partial Regex MyRegex();
}
