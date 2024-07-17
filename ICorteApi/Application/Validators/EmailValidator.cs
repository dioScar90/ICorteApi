using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace ICorteApi.Application.Validators;

public class EmailValidatorAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        return (value is string email) && ValidateEmail(email);
    }
    
    private bool ValidateEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            ErrorMessage = "Email não pode estar vazio";
            return false;
        }
        
        string trimmedLowerEmail = value.Trim().ToLower();
        
        if (trimmedLowerEmail.EndsWith('.'))
        {
            ErrorMessage = "Email incompleto ou com formato inválido";
            return false;
        }

        try
        {
            var addr = new MailAddress(trimmedLowerEmail);

            if (!string.Equals(addr.Address, trimmedLowerEmail, StringComparison.CurrentCultureIgnoreCase))
            {
                ErrorMessage = "Email com formato inválido";
                return false;
            }
            
            return true;
        }
        catch (FormatException)
        {
            ErrorMessage = "Email incompleto ou com formato inválido";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        return false;
    }
}
