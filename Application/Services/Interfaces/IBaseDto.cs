namespace ICorteApi.Application.Services;

public interface IDtoResponse<TEntity> : IDtoResponse, IDto<TEntity>
    where TEntity : class, IBaseTableEntity
{
}

public interface IDtoRequest<TEntity> : IDtoRequest, IDto<TEntity>
    where TEntity : class, IBaseTableEntity
{
}

public interface IDto<TEntity> : IDto
    where TEntity : class, IBaseTableEntity
{
}

public interface IDtoResponse : IDto;
public interface IDtoRequest : IDto;
public interface IDto;
