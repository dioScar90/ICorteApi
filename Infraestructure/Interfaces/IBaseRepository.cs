using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IBaseTableEntity
{
    Task<TEntity?> CreateAsync(TEntity entity);

    Task<TEntity?> GetByIdAsync(params object[] primaryKeys);

    Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> filterId, params Expression<Func<TEntity, object>>[] includes);

    Task<PaginationResponse<TEntity>> GetAllAsync(IPaginationProperties<TEntity> props);

    Task<bool> UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(TEntity entity);
}
