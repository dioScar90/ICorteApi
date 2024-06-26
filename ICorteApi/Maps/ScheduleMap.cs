using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class ScheduleMap : BaseMap<Schedule>
{
    public override void Configure(EntityTypeBuilder<Schedule> builder)
    {
        base.Configure(builder);
    }
}
