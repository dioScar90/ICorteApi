using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class RecurringScheduleMap : BaseMap<RecurringSchedule>
{
    public override void Configure(EntityTypeBuilder<RecurringSchedule> builder)
    {
        base.Configure(builder);

        builder.HasKey(rs => new { rs.DayOfWeek, rs.BarberShopId });

        builder.HasOne(rs => rs.BarberShop)
            .WithMany(b => b.RecurringSchedules)
            .HasForeignKey(rs => rs.BarberShopId)
            .IsRequired(false);
    }
}
