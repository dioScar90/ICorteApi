using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class PersonMap : BaseMap<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(p => p.User)
            .WithOne(u => u.Person)
            .HasForeignKey<Person>(p => p.UserId);

        builder.HasOne(p => p.BarberShop)
            .WithMany(b => b.Barbers)
            .HasForeignKey(p => p.BarberShopId);
    }
}
