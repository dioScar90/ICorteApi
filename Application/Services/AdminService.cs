using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AdminService(AppDbContext context, IAdminErrors errors, IConfiguration configuration) : IAdminService
{
    private readonly AppDbContext _context = context;
    private readonly IAdminErrors _errors = errors;
    private readonly IConfiguration _configuration = configuration;
    
    private string? GetEnvironmentValue(string key) => Environment.GetEnvironmentVariable(key) ?? _configuration[key];

    public async Task RemoveAllRows(string passphrase, string userEmail, bool? evenMasterAdmin = null)
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
            
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Messages.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.Reports.IgnoreQueryFilters().ExecuteDeleteAsync();

            await _context.Appointments.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.Services.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            await _context.SpecialSchedules.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.RecurringSchedules.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.Addresses.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.BarberShops.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            await _context.Profiles
                .IgnoreQueryFilters()
                .Where(p => evenMasterAdmin == true || p.User.Email != userEmail)
                .ExecuteDeleteAsync();

            await _context.Users
                .IgnoreQueryFilters()
                .Where(u => evenMasterAdmin == true || u.Email != userEmail)
                .ExecuteDeleteAsync();
                
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
