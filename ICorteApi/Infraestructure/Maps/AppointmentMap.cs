using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class AppointmentMap : BaseMap<Appointment>
{
    public override void Configure(EntityTypeBuilder<Appointment> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(a => a.Client)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClientId);
        
        builder.HasOne(a => a.BarberShop)
            .WithMany(b => b.Appointments)
            .HasForeignKey(a => a.BarberShopId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(a => a.Services)
            .WithMany(s => s.Appointments)
            .UsingEntity(
                "service_appointment",

                l => l.HasOne(typeof(Service))
                    .WithMany()
                    .HasForeignKey("service_id")
                    .HasPrincipalKey(nameof(Service.Id))
                    .OnDelete(DeleteBehavior.Restrict),

                r => r.HasOne(typeof(Appointment))
                    .WithMany()
                    .HasForeignKey("appointment_id")
                    .HasPrincipalKey(nameof(Appointment.Id))
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
