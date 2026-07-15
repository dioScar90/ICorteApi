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

        builder.HasMany(a => a.ServiceAppointments)
            .WithOne(sa => sa.Appointment)
            .HasForeignKey(sa => sa.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
