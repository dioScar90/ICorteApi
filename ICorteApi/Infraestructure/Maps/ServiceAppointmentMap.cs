using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class ServiceAppointmentMap : BaseMap<ServiceAppointment>
{
    public override void Configure(EntityTypeBuilder<ServiceAppointment> builder)
    {
        base.Configure(builder);

        builder.HasKey(aps => new { aps.AppointmentId, aps.ServiceId });

        builder.HasOne(aps => aps.Appointment)
            .WithMany(a => a.ServiceAppointments)
            .HasForeignKey(aps => aps.AppointmentId);

        builder.HasOne(aps => aps.Service)
            .WithMany(s => s.ServiceAppointments)
            .HasForeignKey(aps => aps.ServiceId);
    }
}
