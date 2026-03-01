namespace ICorteApi.Application.Services;

public interface IEntityService<TEntity, TDtoRequest, TDtoResponse> : IService<TEntity>
    where TEntity : class, IBaseTableEntity
    where TDtoRequest : class, IDtoRequest<TEntity>
    where TDtoResponse : class, IDtoResponse<TEntity>
{
    Task<TDtoResponse> CreateAsync(TDtoRequest dto);

    Task<TEntity> FindEntityAsync(int id, int barberShopId);

    Task<TDtoResponse> GetByIdAsync(int id, int barberShopId);

    Task UpdateAsync(TDtoRequest dto, int id);

    Task DeleteAsync(int id, int barberShopId);
}

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
