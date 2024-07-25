using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public abstract class BasePrimaryKeyRepository<TEntity, TKey>(AppDbContext context)
    : BaseRepository<TEntity>(context), IBasePrimaryKeyRepository<TEntity, TKey>
    where TEntity : class, IPrimaryKeyEntity<TKey>, IBaseTableEntity
{
    public async Task<ISingleResponse<TEntity>> GetByIdAsync(TKey key, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (includes is not null && includes.Length > 0)
            foreach (var include in includes)
                query  = query.Include(include);
                
        var entity = await query.SingleOrDefaultAsync(x => x.Key!.Equals(key));
        // var entity = await query.SingleOrDefaultAsync(x => x.Key == key);

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);
        
        return Response.Success(entity);
    }
}
