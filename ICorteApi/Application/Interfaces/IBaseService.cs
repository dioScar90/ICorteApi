using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBaseService<TEntity> : IService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(TEntity entity);

    Task<ISingleResponse<TEntity>> GetByIdAsync(
        Expression<Func<TEntity, bool>> filterId,
        params Expression<Func<TEntity, object>>[] includes);
        
    Task<ICollectionResponse<TEntity>> GetAllAsync(GetAllProperties<TEntity> props);

    Task<IResponse> UpdateAsync(TEntity entity);

    Task<IResponse> DeleteAsync(TEntity entity);
}
