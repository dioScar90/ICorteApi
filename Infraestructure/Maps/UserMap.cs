using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class UserMap : BaseMap<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        builder
            .HasIndex(u => new { u.Email, u.UserName })
            .IsUnique()
            .HasFilterForDeletedAt(builder);
    }
}
