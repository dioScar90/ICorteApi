using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class ScheduleMap : BaseMap<Schedule>
{
    public override void Configure(EntityTypeBuilder<Schedule> builder)
    {
        base.Configure(builder);

        builder.HasOne(s => s.BarberShop)
            .WithMany(b => b.Schedules)
            .HasForeignKey(s => s.BarberShopId);
        
        builder.HasOne(s => s.Barber)
            .WithMany(b => b.Schedules)
            .HasForeignKey(s => s.BarberId);
    }
}
