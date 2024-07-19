using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class SpecialScheduleMap : BaseMap<SpecialSchedule>
{
    public override void Configure(EntityTypeBuilder<SpecialSchedule> builder)
    {
        base.Configure(builder);

        builder.HasOne(s => s.BarberShop)
            .WithMany(b => b.SpecialSchedules)
            .HasForeignKey(s => s.BarberShopId);
    }
}
