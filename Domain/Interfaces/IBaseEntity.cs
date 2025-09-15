namespace ICorteApi.Domain.Interfaces;

public interface IBaseUserEntity : IBaseEntity<User>
{
    void UpdatedUserNow();
}

public interface IBaseEntity<TEntity> : IBaseEntity, IBaseTableEntity<TEntity>
    where TEntity : class, IBaseTableEntity
{
}

public interface IBaseEntity
{
    int Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsDeleted { get; }

    void DeleteEntity();
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
    void UpdateEntityByDto<TDto>(TDto requestDto, DateTime? utcNow = null) where TDto : IDtoRequest<TEntity>;
    IDtoResponse<TEntity> CreateDto();
}

public interface IBaseTableEntity { }