using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public abstract class BaseCompositeKeyRepository<TEntity, TKey1, TKey2>(AppDbContext context)
    : BaseRepository<TEntity>(context), IBaseCompositeKeyRepository<TEntity, TKey1, TKey2>
    where TEntity : class, ICompositeKeyEntity<TKey1, TKey2>, IBaseTableEntity
{
    public async Task<ISingleResponse<TEntity>> GetByIdAsync(
        Expression<Func<TEntity, bool>> func,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (includes?.Length > 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.SingleOrDefaultAsync(func);

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);

        return Response.Success(entity);
    }

    public override async Task<IResponse> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
