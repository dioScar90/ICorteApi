using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class RecurringScheduleMap : BaseMap<RecurringSchedule>
{
    public override void Configure(EntityTypeBuilder<RecurringSchedule> builder)
    {
        base.Configure(builder);

        builder.HasKey(pcv => new { pcv.DayOfWeek, pcv.BarberShopId });

        builder.HasOne(s => s.BarberShop)
            .WithMany(b => b.RecurringSchedules)
            .HasForeignKey(s => s.BarberShopId);
    }
}
