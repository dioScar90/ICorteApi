using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class SpecialScheduleMap : BaseMap<SpecialSchedule>
{
    public override void Configure(EntityTypeBuilder<SpecialSchedule> builder)
    {
        base.Configure(builder);

        builder.HasKey(ss => new { ss.Date, ss.BarberShopId });

        builder.HasOne(ss => ss.BarberShop)
            .WithMany(b => b.SpecialSchedules)
            .HasForeignKey(ss => ss.BarberShopId)
            .IsRequired(false);
    }
}
