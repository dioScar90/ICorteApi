using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class RecurringScheduleMap : BaseMap<RecurringSchedule>
{
    public override void Configure(EntityTypeBuilder<RecurringSchedule> builder)
    {
        base.Configure(builder);
        
        builder
            .HasIndex(rs => new { rs.DayOfWeek, rs.BarberShopId })
            .IsDescending(true, false)
            .HasFilter($"[{nameof(RecurringSchedule.DeletedAt).ToSnakeCase()}] IS NULL");

        builder
            .HasOne(rs => rs.BarberShop)
            .WithMany(b => b.RecurringSchedules)
            .HasForeignKey(rs => rs.BarberShopId)
            .IsRequired(false);
    }
}
