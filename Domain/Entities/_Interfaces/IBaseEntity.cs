namespace ICorteApi.Domain.Entities;

public interface IBaseUserEntity : IBaseEntity<User>
{
    void UpdatedUserNow();
}

public interface IBaseEntity<TEntity>
    : IBaseEntity, IBaseTableEntity<TEntity>
        where TEntity : class, IBaseTableEntity
{
}

public interface IBaseEntity
{
    int Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    DateTime? DeletedAt { get; }

    void DeleteEntity();
}

public interface IBaseTableEntity<TEntity>
    : IBaseTableEntity
        where TEntity : class, IBaseTableEntity
{
}

public interface IBaseTableEntity {}
