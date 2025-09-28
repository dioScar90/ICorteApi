namespace ICorteApi.Application.Services;

public interface IService<TEntity> : IService
    where TEntity : class, IBaseTableEntity
{
}

public interface IService
{
}

public record PaginationResponse<TEntity>(
    TEntity[] Items,
    int TotalItems,
    int TotalPages,
    int Page,
    int PageSize
);
