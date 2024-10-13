using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class BarberShopMap : BaseMap<BarberShop>
{
    public override void Configure(EntityTypeBuilder<BarberShop> builder)
    {
        base.Configure(builder);

        builder.HasOne(b => b.Owner)
            .WithOne(p => p.BarberShop)
            .HasForeignKey<BarberShop>(b => b.OwnerId);
    }
}
