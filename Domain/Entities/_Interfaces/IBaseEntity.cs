using ICorteApi.Application.Services;

namespace ICorteApi.Domain.Entities;

public interface IBaseUserEntity : IBaseEntity<User, UserDto>
{
    void UpdatedUserNow();
}

public interface IBaseEntity<TEntity, TDtoResponse, TDtoRequest>
    : IBaseEntity, IBaseTableEntity<TEntity, TDtoResponse, TDtoRequest>
        where TEntity : class, IBaseTableEntity
        where TDtoResponse : class, IDtoResponse<TEntity>
        where TDtoRequest : class, IDtoRequest<TEntity>
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

public interface ICompositeKeyEntity<TEntity, TDtoResponse, TDtoRequest>
    : IBaseTableEntity<TEntity, TDtoResponse, TDtoRequest>
        where TEntity : class, IBaseTableEntity
        where TDtoResponse : class, IDtoResponse<TEntity>
        where TDtoRequest : class, IDtoRequest<TEntity>
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsActive { get; }
}

public interface IBaseTableEntity<TEntity, TDtoResponse, TDtoRequest>
    : IBaseTableEntity
        where TEntity : class, IBaseTableEntity
        where TDtoResponse : class, IDtoResponse<TEntity>
        where TDtoRequest : class, IDtoRequest<TEntity>
{
    void UpdateEntity(TDtoRequest dto, DateTime? utcNow = null);
    TDtoResponse CreateDto();
}

public interface IBaseTableEntity
{
    // void UpdateEntity<TDtoRequest, TEntity>(TDtoRequest dto)
    //     where TEntity : class, IBaseTableEntity
    //     where TDtoRequest : class, IDtoRequest<TEntity>;
}
