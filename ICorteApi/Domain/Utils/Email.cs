using System.Net.Mail;

namespace ICorteApi.Domain.Utils;

public class Email(string email)
{
    private readonly MailAddress _value = ValidateEmail(email);
    
    private static MailAddress ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email), "Email não pode estar vazio");
        
        string trimmedLowerEmail = email.Trim().ToLower();
        
        if (trimmedLowerEmail.EndsWith('.'))
            throw new FormatException("Email incompleto ou com formato inválido");

        try {
            var addr = new MailAddress(trimmedLowerEmail);

            if (!string.Equals(addr.Address, trimmedLowerEmail, StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("Email com formato inválido", nameof(email));
            
            return addr;
        }
        // catch (FormatException) {
        //     throw new ArgumentOutOfRangeException(nameof(email), "Email incompleto ou com formato inválidooooooooo");
        // }
        catch (Exception) {
            throw;
        }
    }

    public override string ToString() => _value.Address;
}
