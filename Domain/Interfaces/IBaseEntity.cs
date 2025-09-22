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

public interface ICompositeKeyEntity<TEntity, TDto, TPK1, TPK2> : IBaseTableEntity<TEntity, TDto, TPK1, TPK2>
    where TEntity : class, IBaseTableEntity
    where TDto : IDto<TEntity>
    where TPK1 : object
    where TPK2 : object
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsActive { get; }
}

public interface IBaseTableEntity<TEntity, TDto, TPK1, TPK2> : IBaseTableEntity
    where TEntity : class, IBaseTableEntity
    where TDto : IDto<TEntity>
    where TPK1 : object
    where TPK2 : object
{
    void UpdateEntityByDto(TDto requestDto, DateTime? utcNow = null);
    TDto CreateDto();
}

public interface IBaseTableEntity { }