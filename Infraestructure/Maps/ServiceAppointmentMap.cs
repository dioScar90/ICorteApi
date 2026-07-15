using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class ServiceAppointmentMap : BaseMap<ServiceAppointment>
{
    public override void Configure(EntityTypeBuilder<ServiceAppointment> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(x => new { x.AppointmentId, x.ServiceId })
            .IsDescending(true, true)
            .IsUnique()
            .HasFilterForDeletedAt(builder);

        builder.HasOne(x => x.Appointment)
            .WithMany(x => x.ServiceAppointments)
            .HasForeignKey(x => x.AppointmentId);

        builder.HasOne(x => x.Service)
            .WithMany(x => x.ServiceAppointments)
            .HasForeignKey(x => x.ServiceId);
    }
}
