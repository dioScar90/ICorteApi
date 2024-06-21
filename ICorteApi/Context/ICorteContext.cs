using Microsoft.EntityFrameworkCore;
using ICorteApi.Entities;

namespace ICorteApi.Context;

public class ICorteContext(DbContextOptions<ICorteContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Barber> Barbers { get; set; }
    // public DbSet<Client> Clients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    // public DbSet<Appointment> Appointments { get; set; }
    // public DbSet<Payment> Payments { get; set; }
    // public DbSet<Report> Reports { get; set; }
    // public DbSet<Schedule> Schedules { get; set; }
    // public DbSet<Service> Services { get; set; }
    // public DbSet<Message> Messages { get; set; }
    // public DbSet<Conversation> Conversations { get; set; }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlite($"DataSource = Required{GetType().Name}.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.Barber)
        //     .WithOne(b => b.User)
        //     .HasForeignKey<Barber>(b => b.UserId);

        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.Client)
        //     .WithOne(c => c.User)
        //     .HasForeignKey<Client>(c => c.UserId);

        // modelBuilder.Entity<Appointment>()
        //     .HasOne(a => a.Barber)
        //     .WithMany(b => b.Appointments)
        //     .HasForeignKey(a => a.BarberId);

        // modelBuilder.Entity<Appointment>()
        //     .HasOne(a => a.Client)
        //     .WithMany(c => c.Appointments)
        //     .HasForeignKey(a => a.ClientId);

        // modelBuilder.Entity<Appointment>()
        //     .HasOne(a => a.Payment)
        //     .WithOne(p => p.Appointment)
        //     .HasForeignKey<Payment>(p => p.AppointmentId);

        // modelBuilder.Entity<Appointment>()
        //     .HasMany(a => a.Services)
        //     .WithMany(s => s.Appointments)
        //     .UsingEntity(j => j.ToTable("AppointmentServices"));

        // modelBuilder.Entity<Barber>()
        //     .HasMany(b => b.Schedules)
        //     .WithOne(s => s.Barber)
        //     .HasForeignKey(s => s.BarberId);

        // modelBuilder.Entity<Barber>()
        //     .HasOne(b => b.Address)
        //     .WithOne(b => b.Barber)
        //     .HasForeignKey<Address>(a => a.ClientId);

        // modelBuilder.Entity<Conversation>()
        //     .HasMany(c => c.Messages)
        //     .WithOne(m => m.Conversation)
        //     .HasForeignKey(m => m.ConversationId);
        
        // modelBuilder.Entity<Conversation>()
        //     .HasOne(c => c.Client)
        //     .WithMany()
        //     .HasForeignKey(c => c.ClientId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // modelBuilder.Entity<Conversation>()
        //     .HasOne(c => c.Barber)
        //     .WithMany()
        //     .HasForeignKey(c => c.BarberId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // base.OnModelCreating(modelBuilder);
    }
}
