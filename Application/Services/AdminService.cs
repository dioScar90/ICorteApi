using ICorteApi.Domain.Interfaces;
using ICorteApi.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AdminService(
    AppDbContext context,
    UserManager<User> userManager,
    IBarberScheduleRepository repository,
    IAdminErrors errors,
    IConfiguration configuration) : IAdminService
{
    private readonly AppDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IBarberScheduleRepository _barberScheduleRep = repository;
    private readonly IAdminErrors _errors = errors;
    private readonly IConfiguration _configuration = configuration;
    
    private string? GetEnvironmentValue(string key) => Environment.GetEnvironmentVariable(key) ?? _configuration[key];

    private void CheckEmail(string userEmail)
    {
        var emailHardDelete = GetEnvironmentValue("EMAIL_TO_HARD_DELETE");

        if (string.IsNullOrEmpty(emailHardDelete))
            _errors.ThrowNullEmailException();

        if (userEmail != emailHardDelete)
            _errors.ThrowNotEqualEmailException();
    }

    private void CheckPassphrase(string passphrase)
    {
        var passphraseHardDelete = GetEnvironmentValue("PASSPHRASE_TO_HARD_DELETE");
        
        if (string.IsNullOrEmpty(passphraseHardDelete))
            _errors.ThrowNullPassphaseException();

        if (passphrase != passphraseHardDelete)
            _errors.ThrowNotEqualPassphaseException();
    }
    
    private void CheckPassphraseAndEmail(string userEmail, string passphrase)
    {
        CheckEmail(userEmail);
        CheckPassphrase(passphrase);
    }

    private bool IsPostgres() => _context.Database.ProviderName!.Contains("Postgre", StringComparison.InvariantCultureIgnoreCase);

    private async Task<User?> GetUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

    public async Task ResetPasswordForSomeUser(string passphrase, string userEmail, string emailToBeReseted)
    {
        CheckPassphraseAndEmail(userEmail, passphrase);
        
        var user = await GetUserByEmail(emailToBeReseted);
        
        if (user is null)
            _errors.ThrowUserDoesNotExistException(emailToBeReseted);
            
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            var identityResult = await _userManager.ResetPasswordAsync(user!, token, "Senha@123");

            if (!identityResult.Succeeded)
                _errors.ThrowResetPasswordException(emailToBeReseted, [..identityResult.Errors]);
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveAllRows(string passphrase, string userEmail, bool? evenMasterAdmin = null)
    {
        CheckPassphraseAndEmail(userEmail, passphrase);

        if (!await IsThereAnyUserHere(evenMasterAdmin))
            _errors.ThrowThereIsNobodyToBeDeletedException();
            
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
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    private static User[] GetAllUsersToMock() => DataSeeder.GetAllUsersToMock();
    private static HashSet<string> GetUserRolesToBeSetted(User user) => DataSeeder.GetUserRolesToBeSetted(user);
    
    private async Task<bool> IsThereAnyUserHere(bool? evenMasterAdmin = null) =>
        await _context.Users.AnyAsync(x => evenMasterAdmin == true || x.Email != "diogols@live.com");
    
    private async Task<bool> IsThereAnyAppointmentHere(DateOnly startDate, DateOnly limitDate) =>
        await _context.Appointments.AnyAsync(x => x.Date >= startDate && x.Date <= limitDate);
    
    public async Task PopulateAllInitialTables(string passphrase, string userEmail)
    {
        CheckPassphraseAndEmail(userEmail, passphrase);
        
        if (await IsThereAnyUserHere())
            _errors.ThrowThereAreTooManyPeopleHereException();
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            await _context.Messages.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            foreach (var user in GetAllUsersToMock())
            {
                // Finding if user already exists.
                if ((await _userManager.FindByEmailAsync(user.Email!)) is not null)
                    continue;
                    
                // Trying to create a not already existed user.
                var identityResult = await _userManager.CreateAsync(user, user.GetPasswordToBeHashed());

                if (!identityResult.Succeeded)
                {
                    foreach (var err in identityResult.Errors)
                    {
                        Console.WriteLine("Error.Code => " + err.Code);
                        Console.WriteLine("Error.Description => " + err.Description);
                    }

                    continue;
                }
                
                await _userManager.AddToRolesAsync(user, [..GetUserRolesToBeSetted(user)]);
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    private async Task<int[]> GetAllBarberIds()
    {
        return await _context.BarberShops
			.AsNoTracking()
			.Select(x => x.Id)
			.ToArrayAsync();
    }
    
    private async Task<int[]> GetAllClientIds()
    {
        return await _context.Users
			.AsNoTracking()
			.Where(x => x.Email != "diogols@live.com" && x.BarberShop == null)
			.Select(x => x.Id)
			.ToArrayAsync();
    }
    
    public async Task PopulateWithAppointments(string passphrase, string userEmail, DateOnly? firstDate, DateOnly? limitDate)
    {
        CheckPassphraseAndEmail(userEmail, passphrase);

        DateOnly dayToPopulate = firstDate ?? DateOnly.FromDateTime(DateTime.Now);
        DateOnly VERY_LIMIT_DATE = limitDate ?? dayToPopulate.AddDays(30);

        if (dayToPopulate > VERY_LIMIT_DATE)
            _errors.ThrowLimitDateIsLessThanStartDateException();

        if (!await IsThereAnyUserHere())
            _errors.ThrowThereIsNobodyHereToSetAppointmentsException();
        
        if (await IsThereAnyAppointmentHere(dayToPopulate, VERY_LIMIT_DATE))
            _errors.ThrowThereAreTooManyAppointmentsHereException();
        
        var barberIds = await GetAllBarberIds();
        var clientIds = await GetAllClientIds();
        
        var random = new Random();
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            while (dayToPopulate <= VERY_LIMIT_DATE)
            {
                var lastOneAdded = new Dictionary<int, TimeOnly>();
                var dayOfWeek = (int)dayToPopulate.DayOfWeek;
                var firstDateThisWeek = dayToPopulate.AddDays(dayOfWeek * -1);

                foreach (int clientIdToAdd in clientIds)
                {
                    int barberIdToAdd = barberIds[random.Next(barberIds.Length)];

                    var services = await _context.Services.Where(x => x.BarberShopId == barberIdToAdd).ToArrayAsync();
                    var serviceIds = services.Select(x => x.Id).ToArray();
                    
                    var slots = await _barberScheduleRep.GetAvailableSlotsAsync(barberIdToAdd, dayToPopulate, firstDateThisWeek, serviceIds);

                    if (slots.Length < 3)
                        continue;
                        
                    var startTime = lastOneAdded.TryGetValue(barberIdToAdd, out TimeOnly last)
                        ? slots[1..^1].First(s => s > last.AddMinutes(47))
                        : slots[0];
                    var payment = clientIdToAdd % 3 == 0 ? PaymentType.Card : clientIdToAdd % 2 == 0 ? PaymentType.Transfer : PaymentType.Cash;
                    
                    var newAppoint = new Appointment(new(dayToPopulate, startTime, null, payment, []), services, clientIdToAdd);

                    await _context.Appointments.AddAsync(newAppoint);
                    await _context.SaveChangesAsync();
                    
                    lastOneAdded[barberIdToAdd] = startTime;
                }
                
                dayToPopulate = dayToPopulate.AddDays(1);
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task DeleteServiceAndRemoveFromAllAppointments(string passphrase, string userEmail, int serviceId)
    {
        CheckPassphraseAndEmail(userEmail, passphrase);
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            await _context.Database.ExecuteSqlAsync($"DELETE FROM service_appointment WHERE service_id = {serviceId}");
            await _context.Database.ExecuteSqlAsync($"DELETE FROM services WHERE id = {serviceId}");
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<FoundUserByAdmin[]> SearchForUsersByName(string userEmail, string? name)
    {
        CheckEmail(userEmail);

        if (name is null)
            return [];
        
        bool isPostgre = IsPostgres();
        
        return await _context.Users
            .AsNoTracking()
            .Where(u => isPostgre
            ? (
                EF.Functions.ILike(u.Profile.FirstName, "%" + name + "%")
                || EF.Functions.ILike(u.Profile.LastName, "%" + name + "%")
                || EF.Functions.ILike(u.Email!, "%" + name + "%")
            )
            : (
                EF.Functions.Like(u.Profile.FirstName, "%" + name + "%")
                || EF.Functions.Like(u.Profile.LastName, "%" + name + "%")
                || EF.Functions.Like(u.Email!, "%" + name + "%")
            ))
            .OrderBy(u => u.Profile.FirstName)
            .Select(u => new FoundUserByAdmin(
                u.Id,
                u.Profile.FirstName,
                u.Profile.LastName,
                u.Email!,
                u.PhoneNumber!,
                u.BarberShop != null
            ))
            .ToArrayAsync();
    }
    
    public async Task<FoundUserByAdmin[]> GetLastUsers(string userEmail, int? take = null)
    {
        CheckEmail(userEmail);
        
        take ??= 15;
        int count = Math.Clamp((int)take!, 1, 50);
        
        return await _context.Users
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .Take(count)
            .Select(u => new FoundUserByAdmin(
                u.Id,
                u.Profile.FirstName,
                u.Profile.LastName,
                u.Email!,
                u.PhoneNumber!,
                u.BarberShop != null
            ))
            .ToArrayAsync();
    }
}
