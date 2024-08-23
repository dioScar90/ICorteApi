using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Interfaces;

public interface IBaseUserEntity : IBaseEntity<User>
{
    void UpdatedUserNow();
}

public interface IBaseEntity<TEntity> : IBaseTableEntity<TEntity>
    where TEntity : class, IBaseTableEntity
{
    int Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsDeleted { get; }

    void DeleteEntity();
}

public interface ICompositeKeyEntity<TEntity, TKey1, TKey2> : IBaseTableEntity<TEntity>
    where TEntity : class, IBaseTableEntity
    where TKey1 : IEquatable<TKey1>
    where TKey2 : IEquatable<TKey2>
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsActive { get; }
}

public interface ICompositeKeyEntity<TEntity> : IBaseTableEntity<TEntity>
    where TEntity : class, IBaseTableEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsActive { get; }
}

public interface IBaseTableEntity<TEntity> : IBaseTableEntity
    where TEntity : class, IBaseTableEntity
{
    void UpdateEntityByDto(IDtoRequest<TEntity> requestDto, DateTime? utcNow = null);
    IDtoResponse<TEntity> CreateDto();
}

public interface IBaseTableEntity { }