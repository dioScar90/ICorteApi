using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public static class EntityTypeBuilderExtensions
{
    extension(string value)
    {
        public string SqlNormalize() => value.ToSnakeCase();
    }

    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class, IBaseTableEntity
    {
        /// <summary>
        /// This must be invoked after Configure()
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string GetColumnName(string propertyName)
        {
            var property = builder.Metadata.FindProperty(propertyName);

            if (property is null)
                throw new InvalidOperationException($"Property '{propertyName}' not found on {typeof(TEntity).Name}.");
            
            var table = StoreObjectIdentifier.Table(
                builder.Metadata.GetTableName()!,
                builder.Metadata.GetSchema());
                
            var columnName = property.GetColumnName(table);
            
            if (string.IsNullOrWhiteSpace(columnName))
                throw new InvalidOperationException($"Property '{propertyName}' not found on Entity {typeof(TEntity).Name}.");
                
            return columnName;
        }
        
        private string GetIsNullForHasFilter(string columnName)
        {
            columnName = columnName.SqlNormalize();
            
            columnName = ModelBuildingContext.Provider switch
            {
                DatabaseProvider.SqlServer  => $"[{columnName}]",
                DatabaseProvider.PostgreSQL => $@"""{columnName}""",
                DatabaseProvider.SQLite     => $@"""{columnName}""",
                DatabaseProvider.InMemory   => columnName,
                _ => columnName // Safe fallback
            };
            
            return $"{columnName} IS NULL";
        }
    }
    
    extension<TEntity>(IndexBuilder<TEntity> idxBuilder) where TEntity : class, IBaseTableEntity
    {
        public IndexBuilder<TEntity> HasFilterForDeletedAt(EntityTypeBuilder<TEntity> builder)
        {
            string columnName = nameof(IBaseEntity<>.DeletedAt);
            string sqlFilter = builder.GetIsNullForHasFilter(columnName);
            
            return idxBuilder
                // toda essa merda só para resultar em '[deleted_at] IS NULL' etc.
                .HasFilter(sqlFilter);
        }
    }
}

public abstract class BaseMap<TEntity>
    : IEntityTypeConfiguration<TEntity> where TEntity : class, IBaseTableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        string? currentTableName = builder.Metadata.GetTableName();

        if (!string.IsNullOrEmpty(currentTableName))
        {
            builder.ToTable(currentTableName.SqlNormalize());
        }
        
        if (typeof(BaseEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(nameof(BaseEntity<>.Id)).HasColumnName(nameof(BaseEntity<>.Id).SqlNormalize());
        }
        
        void mapThisProp(string name, Type propertyType)
        {
            builder.Property(name).HasColumnName(name.SqlNormalize());

            if (propertyType == typeof(decimal))
                builder.Property(name).HasPrecision(9, 4);

            if (propertyType.IsEnum && !IsUnableToBecomeString(propertyType))
                builder.Property(name).HasConversion<string>();
        }
        
        var propsToTheEnd = typeof(IBaseEntity<TEntity>).GetProperties()
            .Where(p => p.Name != nameof(IBaseEntity.Id))
            .ToImmutableDictionary(p => p.Name, p => p.PropertyType);
        
        foreach (var prop in typeof(TEntity).GetProperties())
        {
            if (propsToTheEnd.ContainsKey(prop.Name))
                continue;
                
            if (!IsPrimitiveType(prop.PropertyType))
                continue;
                
            if (IsCompositeKeyName(prop.Name))
            {
                builder.Ignore(prop.Name);
                continue;
            }
            
            mapThisProp(prop.Name, prop.PropertyType);
        }
        
        foreach (var (propName, propPropertyType) in propsToTheEnd)
        {
            mapThisProp(propName, propPropertyType);
        }
        
        if (TEntityImplementsIPrimaryKeyEntity())
            builder.HasQueryFilter(x => ((IBaseEntity<TEntity>)x).DeletedAt != null); // same as 'x => x.DeletedAt != null'
    }
    
    private static bool TEntityImplementsIPrimaryKeyEntity() =>
        typeof(IBaseEntity<TEntity>).IsAssignableFrom(typeof(TEntity));
        
    private static bool IsCompositeKeyName(string name) => name.StartsWith("Id") && name.Length > 2;

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

    private static bool IsUnableToBecomeString(Type type)
    {
        // This line is necessary for allow nullable types.
        Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return underlyingType == typeof(DayOfWeek);
    }
}
