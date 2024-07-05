using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class OperatingScheduleMap : BaseMap<OperatingSchedule>
{
    public override void Configure(EntityTypeBuilder<OperatingSchedule> builder)
    {
        base.Configure(builder);

        builder.HasKey(os => new { os.DayOfWeek, os.BarberShopId });

        builder.HasOne(os => os.BarberShop)
            .WithMany(bs => bs.OperatingSchedules)
            .HasForeignKey(os => os.BarberShopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.BarberShop.IsDeleted);
    }
}
