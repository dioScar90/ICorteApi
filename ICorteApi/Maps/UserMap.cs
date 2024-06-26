using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class UserMap : BaseMap<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasOne(u => u.Person)
            .WithOne(p => p.User)
            .HasForeignKey<Person>(p => p.UserId);
    }
}
