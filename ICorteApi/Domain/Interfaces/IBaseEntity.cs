using ICorteApi.Application.Interfaces;

namespace ICorteApi.Domain.Interfaces;

public interface IPrimaryKeyEntity<TEntity, TKey> : IBaseTableEntity<TEntity>
    where TEntity : class, IBaseTableEntity
    where TKey : IEquatable<TKey>
{
    TKey Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsDeleted { get; }

    void DeleteEntity();
}

public interface ICompositeKeyEntity<TEntity, TKey1, TKey2> : IBaseTableEntity<TEntity>
    where TEntity : class, IBaseTableEntity
{
    TKey1 Id1 { get; }
    TKey2 Id2 { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsActive { get; }
}

public interface IBaseTableEntity<TEntity> : IBaseTableEntity
    where TEntity : class, IBaseTableEntity
{
    void UpdateEntityByDto(IDtoRequest<TEntity> requestDto, DateTime? utcNow = null);
}

public interface IBaseTableEntity {}