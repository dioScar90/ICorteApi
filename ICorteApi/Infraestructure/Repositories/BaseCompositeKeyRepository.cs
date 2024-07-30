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
        TKey1 id1, TKey2 id2,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (includes is not null && includes.Length > 0)
            foreach (var include in includes)
                query  = query.Include(include);
                
        var entity = await query.SingleOrDefaultAsync(x => x.Id1.Equals(id1) && x.Id2.Equals(id2));

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);
        
        return Response.Success(entity);
    }
    
    public async Task<ISingleResponse<TEntity>> GetByIdAsync(
        Expression<Func<TEntity, bool>> func,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (includes is not null && includes.Length > 0)
            foreach (var include in includes)
                query  = query.Include(include);
                
        var entity = await query.SingleOrDefaultAsync(func);

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);
        
        return Response.Success(entity);
    }

    // public async Task<ISingleResponse<TEntity>> GetByIdAsync(
    //     TKey1 id1, TKey2 id2,
    //     params Expression<Func<TEntity, object>>[] includes)
    // {
    //     IQueryable<TEntity> query = _dbSet.AsNoTracking();

    //     if (includes is not null && includes.Length > 0)
    //     {
    //         foreach (var include in includes)
    //         {
    //             query = query.Include(include);
    //         }
    //     }

    //     // Create a parameter for the entity type
    //     var parameter = Expression.Parameter(typeof(TEntity), "x");

    //     // Create expressions for Id1 and Id2
    //     var id1Property = typeof(TEntity).GetProperty("Id1")!;
    //     var id2Property = typeof(TEntity).GetProperty("Id2")!;

    //     var id1Expression = Expression.Property(parameter, id1Property);
    //     var id2Expression = Expression.Property(parameter, id2Property);

    //     var id1ValueExpression = Expression.Constant(id1);
    //     var id2ValueExpression = Expression.Constant(id2);

    //     var id1Comparison = Expression.Equal(id1Expression, id1ValueExpression);
    //     var id2Comparison = Expression.Equal(id2Expression, id2ValueExpression);

    //     var andExpression = Expression.AndAlso(id1Comparison, id2Comparison);

    //     // Create the lambda expression for the query
    //     var lambda = Expression.Lambda<Func<TEntity, bool>>(andExpression, parameter);

    //     var entity = await query.SingleOrDefaultAsync(lambda);

    //     if (entity is null)
    //     {
    //         return Response.Failure<TEntity>(Error.TEntityNotFound);
    //     }

    //     return Response.Success(entity);
    // }
}
