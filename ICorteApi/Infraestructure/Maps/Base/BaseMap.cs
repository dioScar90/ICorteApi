using System.Text.RegularExpressions;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public partial class BaseMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IBaseTableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        string? currentTableName = builder.Metadata.GetTableName();

        if (!string.IsNullOrEmpty(currentTableName))
        {
            string newTableName = CamelCaseToSnakeCase(currentTableName);
            builder.ToTable(newTableName);
        }

        foreach (var prop in typeof(TEntity).GetProperties())
        {
            if (prop.Name.StartsWith("Key") || !IsPrimitiveType(prop.PropertyType))
                continue;
            
            string column_name = CamelCaseToSnakeCase(prop.Name);
            builder.Property(prop.Name).HasColumnName(column_name);
        }

        if (TEntityImplementsIBaseCrudEntity())
            builder.HasQueryFilter(x => !((IBaseCrudEntity)x).IsDeleted); // same as 'x => !x.IsDeleted'
    }

    private static bool TEntityImplementsIBaseCrudEntity() => typeof(IBaseCrudEntity).IsAssignableFrom(typeof(TEntity));

    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex MyRegex();
    private static string CamelCaseToSnakeCase(string prop) => MyRegex().Replace(prop, "$1_$2").ToLower();

    private static bool IsPrimitiveType(Type type)
    {
        // This line is necessary for allow nullable types.
        Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        
        return underlyingType.IsPrimitive
            || underlyingType.IsEnum
            || underlyingType == typeof(string)
            || underlyingType == typeof(float)
            || underlyingType == typeof(decimal)
            || underlyingType == typeof(DateTime)
            || underlyingType == typeof(DateOnly)
            || underlyingType == typeof(TimeOnly)
            || underlyingType == typeof(DateTimeOffset)
            || underlyingType == typeof(TimeSpan)
            || underlyingType == typeof(Guid);
    }
}
