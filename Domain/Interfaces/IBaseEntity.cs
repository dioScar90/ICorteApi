namespace ICorteApi.Domain.Interfaces;

public interface IBaseUserEntity : IBaseEntity<User, UserDto>
{
    void UpdatedUserNow();
}

public interface IBaseEntity<TEntity, TDto> : IBaseEntity, IBaseTableEntity<TEntity, TDto>
    where TEntity : class, IBaseTableEntity
    where TDto : IDto<TEntity>
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

public interface ICompositeKeyEntity<TEntity, TDto> : IBaseTableEntity<TEntity, TDto>
    where TEntity : class, IBaseTableEntity
    where TDto : IDto<TEntity>
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsActive { get; }
}

public interface IBaseTableEntity<TEntity, TDto> : IBaseTableEntity
    where TEntity : class, IBaseTableEntity
    where TDto : IDto<TEntity>
{
    void UpdateEntityByDto(TDto requestDto, DateTime? utcNow = null);
    TDto CreateDto();
}

public interface IBaseTableEntity { }