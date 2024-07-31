using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBaseService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(IDtoRequest<TEntity> dto);

    Task<ICollectionResponse<TEntity>> CreateAsync(IDtoRequest<TEntity>[] dtos);
    
    Task<ICollectionResponse<TEntity>> GetAllAsync(int page, int pageSize);
    Task<IResponse> UpdateEntityAsync(TEntity entity);

    Task<IResponse> UpdateEntityAsync(TEntity[] entities);

    Task<IResponse> DeleteEntityAsync(TEntity entity);
}
