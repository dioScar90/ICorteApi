using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class UserMap : BaseMap<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(p => p.BarberShop)
            .WithMany(b => b.Barbers)
            .HasForeignKey(p => p.BarberShopId);
    }
}
