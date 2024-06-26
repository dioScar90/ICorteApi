using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class AppointmentMap : BaseMap<Appointment>
{
    public override void Configure(EntityTypeBuilder<Appointment> builder)
    {
        base.Configure(builder);

        builder.HasOne(a => a.Schedule)
            .WithOne(s => s.Appointment)
            .HasForeignKey<Appointment>(a => a.ScheduleId);

        builder.HasOne(a => a.Client)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClientId);
    }
}
