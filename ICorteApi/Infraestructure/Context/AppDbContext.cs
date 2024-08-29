using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, ApplicationRole, int>(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<BarberShop> BarberShops { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<RecurringSchedule> RecurringSchedules { get; set; }
    public DbSet<SpecialSchedule> SpecialSchedules { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
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
    }

    private void CheckForDeletedUsers()
    {
        var deletedUsers = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
            .Select(e => e.Entity)
            .ToArray();

        foreach (var user in deletedUsers)
        {
            var barberShop = BarberShops
                .FirstOrDefault(bs => !bs.IsDeleted && bs.OwnerId == user.Id);

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
            .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
            .Select(e => e.Entity)
            .ToArray();

        foreach (var barberShop in deletedBarberShops)
        {
            var address = Addresses
                .FirstOrDefault(a => !a.IsDeleted && a.BarberShopId == barberShop.Id);

            if (address is not null)
            {
                address.DeleteEntity();
                Update(address);
            }

            var recurringSchedules = RecurringSchedules
                .Where(rs => rs.BarberShopId == barberShop.Id)
                .ToArray();

            var specialSchedules = SpecialSchedules
                .Where(ss => ss.BarberShopId == barberShop.Id)
                .ToArray();

            var services = Services
                .Where(s => s.BarberShopId == barberShop.Id)
                .ToArray();

            RemoveRange(recurringSchedules, specialSchedules, services);
        }
    }
    
    private void CheckForDeletedServices()
    {
        var deletedServices = ChangeTracker.Entries<Service>()
            .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
            .Select(e => e.Entity)
            .ToArray();
            
        foreach (var service in deletedServices)
            foreach (var appointment in service.Appointments.ToArray())
                service.Appointments.Remove(appointment);
    }

    private void CheckForDeletedAppointments()
    {
        var deletedAppointments = ChangeTracker.Entries<Appointment>()
            .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
            .Select(e => e.Entity)
            .ToArray();
            
        foreach (var appointment in deletedAppointments)
            foreach (var service in appointment.Services.ToArray())
                appointment.Services.Remove(service);
    }
}
