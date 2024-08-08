using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBaseService<TEntity> : IService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateByEntityAsync(TEntity entity);
    Task<ICollectionResponse<TEntity>> GetAllAsync(int? page, int? pageSize);
}
