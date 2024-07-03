using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class ReportMap : BaseMap<Report>
{
    public override void Configure(EntityTypeBuilder<Report> builder)
    {
        base.Configure(builder);

        builder.HasOne(r => r.Client)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.Client.UserId);

        builder.HasOne(p => p.BarberShop)
            .WithMany(b => b.Reports)
            .HasForeignKey(p => p.BarberShopId);
    }
}
