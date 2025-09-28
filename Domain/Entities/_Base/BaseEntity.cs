using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Entities;

public abstract class BaseUserEntity : IdentityUser<int>, IBaseUserEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;
    public abstract void UpdateEntity(UserDto dto, DateTime? utcNow = null);
    public abstract UserDto CreateDto();

    public void UpdatedUserNow() => UpdatedAt = DateTime.UtcNow;

    public void DeleteEntity()
    {
        if (IsDeleted)
            throw new Exception("Já está excluído");

        UpdatedAt = DateTime.UtcNow;
        IsDeleted = true;
    }
}

public abstract class BaseEntity<TEntity, TDtoResponse, TDtoRequest>
    : IBaseEntity<TEntity, TDtoResponse, TDtoRequest>
        where TEntity : class, IBaseTableEntity
        where TDtoResponse : class, IDtoResponse<TEntity>
        where TDtoRequest : class, IDtoRequest<TEntity>
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;
    public abstract void UpdateEntity(TDtoRequest dto, DateTime? utcNow = null);
    public abstract TDtoResponse CreateDto();

    public void DeleteEntity()
    {
        if (IsDeleted)
            throw new Exception("Já está excluído");

        UpdatedAt = DateTime.UtcNow;
        IsDeleted = true;
    }

    protected static string? GetValidStringOrNull(string? value) => string.IsNullOrWhiteSpace(value)
        ? null
        : value.RemoveExtraWhitespaces();

    protected const int MIN_RATING = 1;
    protected const int MAX_RATING = 5;
    protected static int GetValidRatingOrNull(int value) => value is >= MIN_RATING and <= MAX_RATING ? value : Math.Clamp(value, MIN_RATING, MAX_RATING);
}

public abstract class CompositeKeyEntity<TEntity, TDtoResponse, TDtoRequest>
    : ICompositeKeyEntity<TEntity, TDtoResponse, TDtoRequest>
        where TEntity : class, IBaseTableEntity
        where TDtoResponse : class, IDtoResponse<TEntity>
        where TDtoRequest : class, IDtoRequest<TEntity>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; } = true;
    public abstract void UpdateEntity(TDtoRequest dto, DateTime? utcNow = null);
    public abstract TDtoResponse CreateDto();
}
