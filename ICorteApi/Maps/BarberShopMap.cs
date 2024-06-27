using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class BarberMap : BaseMap<BarberShop>
{
    public override void Configure(EntityTypeBuilder<BarberShop> builder)
    {
        base.Configure(builder);

        builder.HasOne(b => b.Owner)
            .WithOne(p => p.OwnedBarberShop)
            .HasForeignKey<BarberShop>(b => b.OwnerId);
    }
}
