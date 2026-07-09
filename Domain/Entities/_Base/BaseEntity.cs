using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Entities;

public abstract class BaseUserEntity : IdentityUser<int>, IBaseUserEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void UpdatedUserNow() => UpdatedAt = DateTime.UtcNow;

    public void DeleteEntity()
    {
        if (DeletedAt is not null)
            throw new Exception("Already deleted");
            
        DeletedAt = UpdatedAt = DateTime.UtcNow;
    }
}

public abstract class BaseEntity<TEntity>
    : IBaseEntity<TEntity>
        where TEntity : class, IBaseTableEntity
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void DeleteEntity()
    {
        if (DeletedAt is not null)
            throw new Exception("Already deleted");

        DeletedAt = UpdatedAt = DateTime.UtcNow;
    }

    protected static string? GetValidStringOrNull(string? value) => string.IsNullOrWhiteSpace(value)
        ? null
        : value.RemoveExtraWhitespaces();

    protected const int MIN_RATING = 1;
    protected const int MAX_RATING = 5;
    protected static int GetValidRatingOrNull(int value) => value is >= MIN_RATING and <= MAX_RATING ? value : Math.Clamp(value, MIN_RATING, MAX_RATING);
}
