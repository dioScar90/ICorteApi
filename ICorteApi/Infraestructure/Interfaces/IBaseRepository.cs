using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(TEntity entity);

    Task<ISingleResponse<TEntity>> GetByIdAsync(params object[] primaryKeys);

    Task<ISingleResponse<TEntity>> GetByIdAsync(
        Expression<Func<TEntity, bool>> filterId,
        params Expression<Func<TEntity, object>>[] includes);
        
    Task<ICollectionResponse<TEntity>> GetAllAsync(IGetAllProperties<TEntity> props);

    Task<IResponse> UpdateAsync(TEntity entity);

    Task<IResponse> DeleteAsync(TEntity entity);
}
