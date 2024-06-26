using Microsoft.EntityFrameworkCore;
using ICorteApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Context;

public class ICorteContext(DbContextOptions<ICorteContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Person> People { get; set; }
    public DbSet<BarberShop> BarberShops { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    // public DbSet<Conversation> Conversations { get; set; }
    // public DbSet<Message> Messages { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<AppointmentService> AppointmentServices { get; set; }
    // public DbSet<Report> Reports { get; set; }

    // public ICorteContext(DbContextOptions<ICorteContext> options) : base(options)
    // {

    // }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // // User and Address
        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.Address)
        //     .WithOne(a => a.User)
        //     .HasForeignKey<Address>(a => a.UserId);

        // // User and Appointments
        // modelBuilder.Entity<Appointment>()
        //     .HasOne(a => a.BarberShop)
        //     .WithMany(u => u.Appointments)
        //     .HasForeignKey(a => a.BarberId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // modelBuilder.Entity<Appointment>()
        //     .HasOne(a => a.Client)
        //     .WithMany(u => u.Appointments)
        //     .HasForeignKey(a => a.ClientId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // // User and Messages
        // modelBuilder.Entity<Message>()
        //     .HasOne(m => m.Sender)
        //     .WithMany(u => u.Messages)
        //     .HasForeignKey(m => m.SenderId);

        // // Conversation and Messages
        // modelBuilder.Entity<Conversation>()
        //     .HasMany(c => c.Messages)
        //     .WithOne(m => m.Conversation)
        //     .HasForeignKey(m => m.ConversationId);

        // // Conversation and Users
        // modelBuilder.Entity<Conversation>()
        //     .HasMany(c => c.Participants)
        //     .WithMany(u => u.Conversations);

        // // User and Schedules
        // modelBuilder.Entity<Schedule>()
        //     .HasOne(s => s.BarberShop)
        //     .WithMany(u => u.Schedules)
        //     .HasForeignKey(s => s.BarberId);

        // // User and Services
        // modelBuilder.Entity<Service>()
        //     .HasOne(s => s.BarberShop)
        //     .WithMany(u => u.Services)
        //     .HasForeignKey(s => s.BarberId);

        // // User and Reports
        // modelBuilder.Entity<Report>()
        //     .HasOne(r => r.User)
        //     .WithMany(u => u.Reports)
        //     .HasForeignKey(r => r.UserId);

        // base.OnModelCreating(modelBuilder);
    }
}
