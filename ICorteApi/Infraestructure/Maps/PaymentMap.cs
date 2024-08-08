using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class PaymentMap : BaseMap<Payment>
{
    public override void Configure(EntityTypeBuilder<Payment> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(pcv => pcv.Appointment)
            .WithMany(c => c.Payments)
            .HasForeignKey(pcv => pcv.AppointmentId);
    }
}
