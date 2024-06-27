
using System.Text.RegularExpressions;
using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

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
            if (IsPrimitiveType(prop.PropertyType))
            {
                string column_name = CamelCaseToSnakeCase(prop.Name);
                builder.Property(prop.Name).HasColumnName(column_name);
            }
        }
        
        if (TEntityImplementsIBaseEntity())
        {
            // var idProperty = builder.Property(nameof(IBaseEntity.Id));
            // var createdAtProperty = builder.Property(nameof(IBaseEntity.CreatedAt));
            // var isDeletedProperty = builder.Property(nameof(IBaseEntity.IsDeleted));
            
            // idProperty.ValueGeneratedOnAdd();
            // createdAtProperty.HasDefaultValue(DateTime.UtcNow);
            // isDeletedProperty.HasDefaultValue(true);

            builder.HasQueryFilter(x => !((IBaseEntity)x).IsDeleted); // same as 'x => !x.IsDeleted'
        }
    }

    private static bool TEntityImplementsIBaseEntity() => typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity));

    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex MyRegex();

    private static string CamelCaseToSnakeCase(string prop) => MyRegex().Replace(prop, "$1_$2").ToLower();

    private static bool IsPrimitiveType(Type type)
    {
        // This line is necessary for allow nullable types.
        Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        if (underlyingType.IsPrimitive)
            return true;

        if (underlyingType.IsEnum)
            return true;
            
        if (underlyingType == typeof(string))
            return true;

        if (underlyingType == typeof(float))
            return true;

        if (underlyingType == typeof(decimal))
            return true;

        if (underlyingType == typeof(DateTime))
            return true;

        if (underlyingType == typeof(DateTimeOffset))
            return true;

        if (underlyingType == typeof(Guid))
            return true;

        return false;
    }
}
