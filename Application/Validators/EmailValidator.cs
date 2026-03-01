using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace ICorteApi.Application.Validators;

public class EmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string email)
            return new ValidationResult(ErrorMessage);
            
        if (string.IsNullOrWhiteSpace(email))
            return new ValidationResult("Email não pode estar vazio");
            
        if (email.EndsWith('.'))
            return new ValidationResult("Email incompleto ou com formato inválido.");
            
        try
        {
            var addr = new MailAddress(email);
            
            if (!string.Equals(addr.Address, email, StringComparison.CurrentCultureIgnoreCase))
                return new ValidationResult("Formato de email inválido.");
                
            return ValidationResult.Success!;
        }
        catch
        {
            return new ValidationResult("Formato de email inválido.");
        }
    }
}
