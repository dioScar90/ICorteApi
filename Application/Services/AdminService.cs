using ICorteApi.Domain.Interfaces;
namespace ICorteApi.Application.Services;

public sealed class AdminService(IAdminRepository repository, IAdminErrors errors) : IAdminService
{
    private readonly IAdminRepository _repository = repository;
    private readonly IAdminErrors _errors = errors;

    public async Task RemoveAllRows(string passphrase, string userEmail)
    {
        var passphraseHardDelete = Environment.GetEnvironmentVariable("PASSPHRASE_HARD_DELETE");
        var emailHardDelete = Environment.GetEnvironmentVariable("EMAIL_HARD_DELETE");

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("passphraseHardDelete => " + passphraseHardDelete);
        Console.WriteLine("emailHardDelete => " + emailHardDelete);
        Console.WriteLine("passphrase => " + passphrase);
        Console.WriteLine("userEmail => " + userEmail);

        if (string.IsNullOrEmpty(passphraseHardDelete))
            _errors.ThrowNullPassphaseException();

        if (string.IsNullOrEmpty(emailHardDelete))
            _errors.ThrowNullEmailException();

        if (passphrase != passphraseHardDelete)
            _errors.ThrowNotEqualPassphaseException();

        if (userEmail != emailHardDelete)
            _errors.ThrowNotEqualEmailException();
        
        await _repository.RemoveAllRows(userEmail);
    }
}
