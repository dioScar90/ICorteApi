namespace ICorteApi.Application.Services;

public interface IDtoResponse<TEntity> : IDto<TEntity>
    where TEntity : class, IBaseTableEntity
{
}

public interface IDtoRequest<TEntity> : IDto<TEntity>
    where TEntity : class, IBaseTableEntity
{
}

public interface IDto<TEntity>
    where TEntity : class, IBaseTableEntity
{
}
