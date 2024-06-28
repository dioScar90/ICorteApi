using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class PersonMap : BaseMap<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.HasOne(p => p.BarberShop)
            .WithMany(b => b.Barbers)
            .HasForeignKey(p => p.BarberShopId);
    }
}
