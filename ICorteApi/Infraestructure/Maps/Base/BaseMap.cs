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
            if (!IsPrimitiveType(prop.PropertyType))
                continue;

            if (IsCompositeKeyName(prop.Name))
            {
                builder.Ignore(prop.Name);
                continue;
            }
            
            string column_name = CamelCaseToSnakeCase(prop.Name);
            builder.Property(prop.Name).HasColumnName(column_name);

            if (prop.PropertyType.IsEnum)
                builder.Property(prop.Name).HasConversion<string>();
        }

        if (TEntityImplementsIPrimaryKeyEntity())
        {
            builder.Property(x => ((IPrimaryKeyEntity<int>)x).Id)
                .ValueGeneratedNever();
            
            builder.HasQueryFilter(x => !((IPrimaryKeyEntity<int>)x).IsDeleted); // same as 'x => !x.IsDeleted'
        }
    }

    private static bool TEntityImplementsIPrimaryKeyEntity() =>
        typeof(IPrimaryKeyEntity<int>).IsAssignableFrom(typeof(TEntity));
        
    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex MyRegex();
    private static string CamelCaseToSnakeCase(string name) => MyRegex().Replace(name, "$1_$2").ToLower();

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

    // private static Dictionary<string, string>? GetKeyMappings<TEntity, TKey1, TKey2>()
    //     where TEntity : ICompositeKeyEntity<TKey1, TKey2>
    // {
    //     var keyMappings = new Dictionary<string, string>();

    //     // Get all properties of the class
    //     var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //     // Find the properties that match TKey1 and TKey2
    //     var id1Property = properties.FirstOrDefault(p => p.PropertyType == typeof(TKey1) && p.Name == "Id1");
    //     var id2Property = properties.FirstOrDefault(p => p.PropertyType == typeof(TKey2) && p.Name == "Id2");

    //     if (id1Property != null)
    //     {
    //         var realId1Property = properties.FirstOrDefault(p => p.PropertyType == typeof(TKey1) && p.Name != "Id1");
    //         if (realId1Property != null)
    //         {
    //             keyMappings.Add("Id1", realId1Property.Name);
    //         }
    //     }

    //     if (id2Property != null)
    //     {
    //         var realId2Property = properties.FirstOrDefault(p => p.PropertyType == typeof(TKey2) && p.Name != "Id2");
    //         if (realId2Property != null)
    //         {
    //             keyMappings.Add("Id2", realId2Property.Name);
    //         }
    //     }

    //     return keyMappings;
    // }
}
