using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

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
