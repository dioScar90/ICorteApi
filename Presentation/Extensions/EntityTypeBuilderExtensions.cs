using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Presentation.Extensions;

public static partial class EntityTypeBuilderExtensions
{
    extension(EntityTypeBuilder builder)
    {
        public EntityTypeBuilder HasFilter(string filter) => builder.HasFilter(filter);
    }
}
