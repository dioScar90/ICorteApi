using System.ComponentModel.DataAnnotations;

namespace ICorteApi.Application.Validators;

public class AdminPassPhraseAttribute : ValidationAttribute
{
    private const string ENVIRONMENT_VARIABLE_KEY = "PASSPHRASE_TO_HARD_DELETE";
    
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success!;
            
        if (value is not string passphrase)
            return new ValidationResult("Invalid passphrase.");
            
        var passphraseHardDelete = GetPassphrase(validationContext);
        
        if (string.IsNullOrWhiteSpace(passphraseHardDelete))
            return new ValidationResult("No inner phrase, no chance.");
            
        if (passphrase != passphraseHardDelete)
            return new ValidationResult("This phrase is totally different from what we agreed upon.");

        return ValidationResult.Success!;
    }
    
    private static string? GetPassphrase(ValidationContext validationContext)
    {
        string? pass = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_KEY);

        if (!string.IsNullOrWhiteSpace(pass))
            return pass;
        
        var configuration = (IConfiguration?)validationContext.GetService(typeof(IConfiguration));
        pass = configuration?[ENVIRONMENT_VARIABLE_KEY];

        return pass;
    }
}
