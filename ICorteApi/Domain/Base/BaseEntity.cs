using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
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

public abstract class BaseEntity<TEntity> : IBaseEntity<TEntity>
    where TEntity : class, IBaseTableEntity
{
    public int Id { get; init; }
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

public abstract class CompositeKeyEntity<TEntity> : ICompositeKeyEntity<TEntity>
    where TEntity : class, IBaseTableEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; } = true;
    public abstract void UpdateEntityByDto(IDtoRequest<TEntity> requestDto, DateTime? utcNow = null);
    public abstract IDtoResponse<TEntity> CreateDto();
}
