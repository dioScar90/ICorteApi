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
    
    private bool IsPostgres() => _context.Database.ProviderName!.Contains("Postgre", StringComparison.InvariantCultureIgnoreCase);

    public async Task<bool> UserExists(
        string email,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context
            .Users
            .AnyAsync(x => x.Email == email, cancellationToken);
    }
        
    private async Task<User?> GetUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

    public async Task<IdentityResult?> ResetPasswordForSomeUser(
        string emailToBeReseted,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await GetUserByEmail(emailToBeReseted);

        if (user is null)
            return null;
        
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            var identityResult = await _userManager.ResetPasswordAsync(user!, token, "Senha@123");

            if (identityResult.Succeeded)
                await transaction.CommitAsync(cancellationToken);
                
            return identityResult;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task RemoveAllRows(
        string userEmail, bool evenMasterAdmin = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _context.Messages
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            await _context.Reports
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);

            await _context.Appointments
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            await _context.Services
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            
            await _context.SpecialSchedules
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            await _context.RecurringSchedules
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            await _context.Addresses
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            await _context.BarberShops
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            
            await _context.Profiles
                .IgnoreQueryFilters()
                .Where(p => evenMasterAdmin || p.User.Email != userEmail)
                .ExecuteDeleteAsync(cancellationToken);

            await _context.Users
                .IgnoreQueryFilters()
                .Where(u => evenMasterAdmin || u.Email != userEmail)
                .ExecuteDeleteAsync(cancellationToken);
                
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    private static User[] GetAllUsersToMock() => DataSeeder.GetAllUsersToMock();
    private static HashSet<string> GetUserRolesToBeSetted(User user) => DataSeeder.GetUserRolesToBeSetted(user);
    
    public async Task<bool> IsThereAnyUserHere(
        bool evenMasterAdmin = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context.Users
            .AnyAsync(x => evenMasterAdmin || x.Email != "diogols@live.com", cancellationToken);
    }
    
    public async Task<bool> IsThereAnyAppointmentHere(
        PeriodToPopulateDto dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context.Appointments
            .AnyAsync(x => x.Date >= dto.DayToPopulate && x.Date <= dto.VeryLimitDate, cancellationToken);
    }
    
    public async Task PopulateAllInitialTables(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            await _context.Messages
                .IgnoreQueryFilters()
                .ExecuteDeleteAsync(cancellationToken);
            
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
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    private async Task<(int[] barberIds, int[] clientIds)> GetBarberShopAndClientIds(
        CancellationToken cancellationToken = default)
    {
        return (
            await _context.BarberShops
                .AsNoTracking()
                .Select(x => x.Id)
                .ToArrayAsync(cancellationToken) ?? [],
                
            await _context.Users
                .AsNoTracking()
                .Where(x => x.BarberShop == null && x.Email != "diogols@live.com")
                .Select(x => x.Id)
                .ToArrayAsync(cancellationToken) ?? []
        );
    }
    
    public async Task PopulateWithAppointments(
        PeriodToPopulateDto dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var random = new Random();
        var (barberIds, clientIds) = await GetBarberShopAndClientIds(cancellationToken);
        
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            while (dto.DayToPopulate <= dto.VeryLimitDate)
            {
                var lastOneAdded = new Dictionary<int, TimeOnly>();
                var dayOfWeek = (int)dto.DayToPopulate.DayOfWeek;
                
                foreach (int clientIdToAdd in clientIds)
                {
                    int barberIdToAdd = barberIds[random.Next(barberIds.Length)];

                    var services = await _context.Services
                        .Where(x => x.BarberShopId == barberIdToAdd)
                        .ToArrayAsync(cancellationToken);

                    var serviceIds = services.Select(x => x.Id).ToArray();
                    
                    var slots = await _barberScheduleRep.GetAvailableSlotsAsync(barberIdToAdd, dto.DayToPopulate, serviceIds);

                    if (slots.Length < 3)
                        continue;
                        
                    var startTime = lastOneAdded.TryGetValue(barberIdToAdd, out TimeOnly last)
                        ? slots[1..^1].First(s => s > last.AddMinutes(47))
                        : slots[0];

                    var payment = clientIdToAdd % 3 == 0 ? PaymentType.Card : clientIdToAdd % 2 == 0 ? PaymentType.Transfer : PaymentType.Cash;
                    
                    var newAppoint = new Appointment(
                        new(
                            clientIdToAdd,
                            barberIdToAdd,
                            dto.DayToPopulate,
                            startTime,
                            TimeSpan.Zero,
                            null,
                            payment,
                            0M,
                            [..services.Select(s => new ServiceForUpdateAppointmentDtoRequest(s.Id))]
                        ), services);
                        
                    await _context.Appointments.AddAsync(newAppoint, cancellationToken);
                    await _context.SaveChangesAsync();
                    
                    lastOneAdded[barberIdToAdd] = startTime;
                }
                
                dto = dto with { DayToPopulate = dto.DayToPopulate.AddDays(1) };
            }
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task DeleteServiceAndRemoveFromAllAppointments(
        int serviceId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            await _context.Database
                .ExecuteSqlAsync($"DELETE FROM service_appointment WHERE service_id = {serviceId}", cancellationToken);

            await _context.Services
                .Where(x => x.Id == serviceId)
                .ExecuteDeleteAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task<FoundUserByAdmin[]> SearchForUsersByName(
        string? name,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (name is null)
            return [];
        
        bool isPostgres = IsPostgres();

        var query = _context.Users
            .AsNoTracking();
            
        if (isPostgres)
        {
            query = query.Where(u => EF.Functions.ILike(u.Profile.FirstName, "%" + name + "%")
                || EF.Functions.ILike(u.Profile.LastName, "%" + name + "%")
                || EF.Functions.ILike(u.Email!, "%" + name + "%"));
        }
        else
        {
            query = query.Where(u => EF.Functions.Like(u.Profile.FirstName, "%" + name + "%")
                || EF.Functions.Like(u.Profile.LastName, "%" + name + "%")
                || EF.Functions.Like(u.Email!, "%" + name + "%"));
        }
        
        return await query
            .OrderBy(u => u.Profile.FirstName)
            .Select(u => new FoundUserByAdmin(
                u.Id,
                u.Profile.FirstName,
                u.Profile.LastName,
                u.Email!,
                u.PhoneNumber!,
                u.BarberShop != null
            ))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<FoundUserByAdmin[]> GetLastUsers(
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            .ToArrayAsync(cancellationToken);
    }
}
