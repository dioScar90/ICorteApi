using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class AppointmentServiceMap : BaseMap<AppointmentService>
{
    public override void Configure(EntityTypeBuilder<AppointmentService> builder)
    {
        base.Configure(builder);

        builder.HasKey(aps => new { aps.AppointmentId, aps.ServiceId });

        builder.HasOne(aps => aps.Appointment)
            .WithMany(a => a.AppointmentServices)
            .HasForeignKey(aps => aps.AppointmentId);

        builder.HasOne(aps => aps.Service)
            .WithMany(s => s.AppointmentServices)
            .HasForeignKey(aps => aps.ServiceId);
    }
}
