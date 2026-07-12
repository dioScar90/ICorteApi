using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Context;

public enum DatabaseProvider
{
    SqlServer,
    PostgreSQL,
    SQLite,
    InMemory
};

public static class ModelBuildingContext
{
    private static readonly AsyncLocal<DatabaseProvider?> _provider = new();

    public static DatabaseProvider Provider =>
        _provider.Value ?? throw new InvalidOperationException(
            "Provider not allowed to be used outside OnModelCreating.");

    internal static void Set(DatabaseProvider provider)
        => _provider.Value = provider;

    internal static void Clear()
        => _provider.Value = null;
}

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, ApplicationRole, int>(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<BarberShop> BarberShops { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceAppointment> ServiceAppointments { get; set; }
    public DbSet<RecurringSchedule> RecurringSchedules { get; set; }
    public DbSet<SpecialSchedule> SpecialSchedules { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    private DatabaseProvider GetDatabaseProvider()
    {
        if (Database.IsSqlServer())
            return DatabaseProvider.SqlServer;

        if (Database.IsNpgsql())
            return DatabaseProvider.PostgreSQL;

        if (Database.IsSqlite())
            return DatabaseProvider.SQLite;

        return DatabaseProvider.InMemory;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ModelBuildingContext.Set(GetDatabaseProvider());
        
        try
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        finally
        {
            ModelBuildingContext.Clear();
        }
    }

    public override int SaveChanges()
    {
        HandleSoftDelete();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSoftDelete();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void HandleSoftDelete()
    {
        CheckForDeletedUsers();
        CheckForDeletedBarberShops();
        CheckForDeletedServices();
        CheckForDeletedAppointments();

        var entities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entities)
        {
            if (entry.Entity is IBaseEntity baseEntity)
            {
                entry.State = EntityState.Modified;
                baseEntity.DeleteEntity();
            }
        }
    }

    private void CheckForDeletedUsers()
    {
        var deletedUsers = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Modified && e.Entity.DeletedAt != null)
            .Select(e => e.Entity)
            .ToArray();

        foreach (var user in deletedUsers)
        {
            var barberShop = BarberShops
                .FirstOrDefault(bs => bs.DeletedAt == null && bs.OwnerId == user.Id);

            if (barberShop is not null)
            {
                barberShop.DeleteEntity();
                Update(barberShop);
            }
        }
    }

    private void CheckForDeletedBarberShops()
    {
        var deletedBarberShops = ChangeTracker.Entries<BarberShop>()
            .Where(e => e.State == EntityState.Modified && e.Entity.DeletedAt != null)
            .Select(e => e.Entity)
            .ToArray();

        foreach (var barberShop in deletedBarberShops)
        {
            var address = Addresses
                .FirstOrDefault(a => a.DeletedAt == null && a.BarberShopId == barberShop.Id);

            if (address is not null)
            {
                address.DeleteEntity();
                Update(address);
            }

            var services = Services
                .Where(s => s.BarberShopId == barberShop.Id)
                .ToArray() ?? [];

            if (services.Length > 0)
                RemoveRange([..services]);

            var specialSchedules = SpecialSchedules
                .Where(ss => ss.BarberShopId == barberShop.Id)
                .ToArray() ?? [];

            if (specialSchedules.Length > 0)
                RemoveRange([..specialSchedules]);

            var recurringSchedules = RecurringSchedules
                .Where(rs => rs.BarberShopId == barberShop.Id)
                .ToArray() ?? [];

            if (recurringSchedules.Length > 0)
                RemoveRange([..recurringSchedules]);
        }
    }
    
    private void CheckForDeletedServices()
    {
        var deletedServices = ChangeTracker.Entries<Service>()
            .Where(e => e.State == EntityState.Modified && e.Entity.DeletedAt != null)
            .Select(e => e.Entity)
            .ToArray();
            
        foreach (var service in deletedServices)
        {
            foreach (var appointment in service.Appointments)
            {
                if (appointment.DeletedAt == null)
                {
                    service.Appointments.Remove(appointment);
                }
            }

            foreach (var appointment in service.Appointments)
            {
                if (appointment.DeletedAt == null)
                {
                    appointment.UpdatePriceAndDuration();
                }
            }
        }
    }

    private void CheckForDeletedAppointments()
    {
        var deletedAppointments = ChangeTracker.Entries<Appointment>()
            .Where(e => e.State == EntityState.Modified && e.Entity.DeletedAt != null)
            .Select(e => e.Entity)
            .ToArray();
            
        foreach (var appointment in deletedAppointments)
            foreach (var service in appointment.Services.ToArray())
                appointment.Services.Remove(service);
    }
}
