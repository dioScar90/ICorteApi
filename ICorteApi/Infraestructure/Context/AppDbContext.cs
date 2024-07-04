using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options), IAppDbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<BarberShop> BarberShops { get; set; }
    public DbSet<OperatingSchedule> OperatingSchedules { get; set; }
    public DbSet<Address> Addresses { get; set; }
    // public DbSet<Appointment> Appointments { get; set; }
    // public DbSet<Conversation> Conversations { get; set; }
    // public DbSet<Message> Messages { get; set; }
    // public DbSet<Schedule> Schedules { get; set; }
    // public DbSet<RecurringSchedule> RecurringSchedules { get; set; }
    // public DbSet<Service> Services { get; set; }
    // public DbSet<AppointmentService> AppointmentServices { get; set; }
    // public DbSet<PersonConversation> PersonConversations { get; set; }
    // public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
