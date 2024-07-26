using ICorteApi.Application.Interfaces;

namespace ICorteApi.Domain.Interfaces;

public interface IPrimaryKeyEntity<TKey> : IBaseTableEntity
    where TKey : IEquatable<TKey>
{
    TKey Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsDeleted { get; set; }
}

public interface ICompositeKeyEntity<TKey1, TKey2> : IBaseTableEntity
    // where TKey1 : IEquatable<TKey1>
    // where TKey2 : IEquatable<TKey2>
{
    TKey1 Id1 { get; set; }
    TKey2 Id2 { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsActive { get; set; }
}

public interface IBaseTableEntity {}