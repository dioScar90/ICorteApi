using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class SpecialScheduleMap : BaseMap<SpecialSchedule>
{
    public override void Configure(EntityTypeBuilder<SpecialSchedule> builder)
    {
        base.Configure(builder);
        
        builder
            .HasIndex(rs => new { rs.Date, rs.BarberShopId })
            .IsDescending(true, false)
            .HasFilter($"[{nameof(SpecialSchedule.DeletedAt).ToSnakeCase()}] IS NULL");

        builder.HasOne(ss => ss.BarberShop)
            .WithMany(b => b.SpecialSchedules)
            .HasForeignKey(ss => ss.BarberShopId)
            .IsRequired(false);
    }
}
