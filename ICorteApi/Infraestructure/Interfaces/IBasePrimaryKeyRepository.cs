using System.Linq.Expressions;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBasePrimaryKeyRepository<TEntity, TKey> : IBaseRepository<TEntity>
    where TEntity : class, IPrimaryKeyEntity<TKey>, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> GetByIdAsync(TKey key, params Expression<Func<TEntity, object>>[]? includes);
}
