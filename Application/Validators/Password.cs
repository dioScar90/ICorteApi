using System.ComponentModel.DataAnnotations;

namespace ICorteApi.Application.Validators;

public class PasswordAttribute(int minLength = 8, bool skipCharChecks = false) : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string password)
            return new ValidationResult(ErrorMessage);
            
        if (string.IsNullOrEmpty(password))
            return new ValidationResult("Senha não pode estar vazia.");

        if (password.Length < minLength)
            return new ValidationResult($"Senha deve conter pelo menos {minLength} caracteres.");

        if (skipCharChecks)
            return ValidationResult.Success!;

        if (!ContainsUppercase(password))
            return new ValidationResult("Senha deve conter pelo menos uma letra maiúscula.");

        if (!ContainsLowercase(password))
            return new ValidationResult("Senha deve conter pelo menos uma letra minúscula.");

        if (!ContainsDigit(password))
            return new ValidationResult("Senha deve conter pelo menos um dígito.");

        if (!ContainNonAlphanumeric(password))
            return new ValidationResult("Senha deve conter pelo menos um caractere especial.");

        return ValidationResult.Success!;
    }
    
    private static bool IsNull(string? value) => value is null;

    private static bool ContainsUppercase(string? password) => !IsNull(password) && password!.Any(char.IsUpper);
    private static bool ContainsLowercase(string? password) => !IsNull(password) && password!.Any(char.IsLower);
    private static bool ContainsDigit(string? password) => !IsNull(password) && password!.Any(char.IsDigit);
    private static bool ContainNonAlphanumeric(string? password) => !IsNull(password) && password!.Any(ch => !char.IsLetterOrDigit(ch));
}
