using ICorteApi.Domain.Errors;
using ICorteApi.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AdminService(
    AppDbContext context,
    ILogger<AdminService> logger,
    UserManager<User> userManager,
    BarberScheduleService barberScheduleRep,
    AdminErrors errors,
    IConfiguration configuration)
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<AdminService> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;
    private readonly BarberScheduleService _barberScheduleRep = barberScheduleRep;
    private readonly AdminErrors _errors = errors;
    private readonly IConfiguration _configuration = configuration;
    
    private string? GetEnvironmentValue(string key) => Environment.GetEnvironmentVariable(key) ?? _configuration[key];
    
    public bool IsAllowableAdminEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
            
        var emailHardDelete = GetEnvironmentValue("EMAIL_TO_HARD_DELETE");
        return email == emailHardDelete;
    }

    public bool IsCorrectAdminPassphrase(string? passphrase)
    {
        if (string.IsNullOrEmpty(passphrase))
            return false;

        var passphraseHardDelete = GetEnvironmentValue("PASSPHRASE_TO_HARD_DELETE");
        return passphrase == passphraseHardDelete;
    }
    
    private void CheckPassphraseAndEmail(string userEmail, string passphrase)
    {
        CheckEmail(userEmail);
        CheckPassphrase(passphrase);
    }

    private bool IsPostgres() => _context.Database.ProviderName!.Contains("Postgre", StringComparison.InvariantCultureIgnoreCase);

    public async Task<bool> UserExists(string email) => await _context
        .Users
        .AnyAsync(x => x.Email == email);
        
    private async Task<User?> GetUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

    public async Task<IdentityResult?> ResetPasswordForSomeUser(string emailToBeReseted)
    {
        var user = await GetUserByEmail(emailToBeReseted);

        if (user is null)
            return null;
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            var identityResult = await _userManager.ResetPasswordAsync(user!, token, "Senha@123");

            if (identityResult.Succeeded)
                await transaction.CommitAsync();
                
            return identityResult;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveAllRows(string userEmail, bool? evenMasterAdmin = null)
    {
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
    
    public async Task<bool> IsThereAnyUserHere(bool? evenMasterAdmin = null) =>
        await _context.Users.AnyAsync(x => evenMasterAdmin == true || x.Email != "diogols@live.com");
    
    public async Task<bool> IsThereAnyAppointmentHere(PeriodToPopulateDto dto) =>
        await _context.Appointments.AnyAsync(x => x.Date >= dto.DayToPopulate && x.Date <= dto.VeryLimitDate);
    
    public async Task PopulateAllInitialTables()
    {
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
    
    public async Task PopulateWithAppointments(PeriodToPopulateDto dto)
    {
        var barberIds = await GetAllBarberIds();
        var clientIds = await GetAllClientIds();
        
        var random = new Random();
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            while (dto.DayToPopulate <= dto.VeryLimitDate)
            {
                var lastOneAdded = new Dictionary<int, TimeOnly>();
                var dayOfWeek = (int)dto.DayToPopulate.DayOfWeek;
                var firstDateThisWeek = dto.DayToPopulate.AddDays(dayOfWeek * -1);

                foreach (int clientIdToAdd in clientIds)
                {
                    int barberIdToAdd = barberIds[random.Next(barberIds.Length)];

                    var services = await _context.Services.Where(x => x.BarberShopId == barberIdToAdd).ToArrayAsync();
                    var serviceIds = services.Select(x => x.Id).ToArray();
                    
                    var slots = await _barberScheduleRep.GetAvailableSlotsAsync(barberIdToAdd, dto.DayToPopulate, firstDateThisWeek, serviceIds);

                    if (slots.Length < 3)
                        continue;
                        
                    var startTime = lastOneAdded.TryGetValue(barberIdToAdd, out TimeOnly last)
                        ? slots[1..^1].First(s => s > last.AddMinutes(47))
                        : slots[0];
                    var payment = clientIdToAdd % 3 == 0 ? PaymentType.Card : clientIdToAdd % 2 == 0 ? PaymentType.Transfer : PaymentType.Cash;
                    
                    var newAppoint = new Appointment(new(dto.DayToPopulate, startTime, null, payment, []), services, clientIdToAdd);

                    await _context.Appointments.AddAsync(newAppoint);
                    await _context.SaveChangesAsync();
                    
                    lastOneAdded[barberIdToAdd] = startTime;
                }
                
                dto = dto with { DayToPopulate = dto.DayToPopulate.AddDays(1) };
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task DeleteServiceAndRemoveFromAllAppointments(int serviceId)
    {
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
    
    public async Task<FoundUserByAdmin[]> SearchForUsersByName(string? name)
    {
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
    
    public async Task<FoundUserByAdmin[]> GetLastUsers(int? take = null)
    {
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
