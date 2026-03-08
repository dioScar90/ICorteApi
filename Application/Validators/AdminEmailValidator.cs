using System.ComponentModel.DataAnnotations;

namespace ICorteApi.Application.Validators;

public class AdminEmailAttribute : ValidationAttribute
{
    private const string ENVIRONMENT_VARIABLE_KEY = "EMAIL_TO_HARD_DELETE";
    
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success!;
            
        if (value is not string email)
            return new ValidationResult("Invalid email.");
            
        var emailHardDelete = GetPassphrase(validationContext);
        
        if (string.IsNullOrWhiteSpace(emailHardDelete))
            return new ValidationResult("No inner email, no chance.");
            
        if (email != emailHardDelete)
            return new ValidationResult("This email not even exists, you vagabond.");
            
        return ValidationResult.Success!;
    }
    
    private static string? GetPassphrase(ValidationContext validationContext)
    {
        string? email = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_KEY);

        if (!string.IsNullOrWhiteSpace(email))
            return email;
        
        var configuration = (IConfiguration?)validationContext.GetService(typeof(IConfiguration));
        email = configuration?[ENVIRONMENT_VARIABLE_KEY];

        return email;
    }
}
