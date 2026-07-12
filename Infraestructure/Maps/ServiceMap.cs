using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class ServiceMap : BaseMap<Service>
{
    public override void Configure(EntityTypeBuilder<Service> builder)
    {
        base.Configure(builder);

        builder.HasOne(s => s.BarberShop)
            .WithMany(b => b.Services)
            .HasForeignKey(s => s.BarberShopId);

        builder.HasMany(s => s.ServiceAppointments)
            .WithOne(sa => sa.Service)
            .HasForeignKey(sa => sa.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
