using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Base;

public abstract class BaseUserEntity : IdentityUser<int>, IBaseUserEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;
    
    public void UpdateEntityByDto(IDtoRequest<User> requestDto, DateTime? utcNow = null) => throw new NotImplementedException();
    public abstract IDtoResponse<User> CreateDto();
    
    public void UpdatedUserNow() => UpdatedAt = DateTime.UtcNow;

    public void DeleteEntity()
    {
        if (IsDeleted)
            throw new Exception("Já está excluído");
        
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = true;
    }
}

public abstract class BasePrimaryKeyEntity<TEntity, TKey> : IPrimaryKeyEntity<TEntity, TKey>
    where TEntity : class, IBaseTableEntity
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;
    public abstract void UpdateEntityByDto(IDtoRequest<TEntity> requestDto, DateTime? utcNow = null);
    public abstract IDtoResponse<TEntity> CreateDto();

    public void DeleteEntity()
    {
        if (IsDeleted)
            throw new Exception("Já está excluído");
        
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = true;
    }
}

public abstract class CompositeKeyEntity<TEntity, TKey1, TKey2> : ICompositeKeyEntity<TEntity, TKey1, TKey2>
    where TEntity : class, IBaseTableEntity
{
    public TKey1 Id1 { get; init; }
    public TKey2 Id2 { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; } = true;
    public abstract void UpdateEntityByDto(IDtoRequest<TEntity> requestDto, DateTime? utcNow = null);
    public abstract IDtoResponse<TEntity> CreateDto();
}
