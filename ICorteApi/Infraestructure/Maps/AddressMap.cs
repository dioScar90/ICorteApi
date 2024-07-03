using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class AddressMap : BaseMap<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);

        builder.HasOne(a => a.BarberShop)
            .WithOne(b => b.Address)
            .HasForeignKey<Address>(a => a.BarberShopId);
    }
}
