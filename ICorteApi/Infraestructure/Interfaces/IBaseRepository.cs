using System.Linq.Expressions;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(TEntity entity);

    Task<ICollectionResponse<TEntity>> CreateAsync(TEntity[] entities);
    
    Task<ICollectionResponse<TEntity>> GetAllAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[]? includes);

    Task<IResponse> UpdateAsync(TEntity entity);

    Task<IResponse> UpdateAsync(TEntity[] entities);

    Task<IResponse> DeleteAsync(TEntity entity);
}
