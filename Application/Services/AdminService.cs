using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AdminService(IAdminRepository repository, IAdminErrors errors, IConfiguration configuration) : IAdminService
{
    private readonly IAdminRepository _repository = repository;
    private readonly IAdminErrors _errors = errors;
    private readonly IConfiguration _configuration = configuration;
    
    private string? GetEnvironmentValue(string key) => Environment.GetEnvironmentVariable(key) ?? _configuration[key];

    public async Task RemoveAllRows(string passphrase, string userEmail)
    {
        var passphraseHardDelete = GetEnvironmentValue("PASSPHRASE_TO_HARD_DELETE");
        var emailHardDelete = GetEnvironmentValue("EMAIL_TO_HARD_DELETE");
        
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
