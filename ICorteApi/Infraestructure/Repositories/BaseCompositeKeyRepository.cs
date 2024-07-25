using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public abstract class BaseCompositeKeyRepository<TEntity, TKey1, TKey2>(AppDbContext context)
    : IBaseCompositeKeyRepository<TEntity, TKey1, TKey2>
    where TEntity : class, ICompositeKeyEntity<TKey1, TKey2>, IBaseTableEntity
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    
    public async Task<IResponse> CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }
    
    public async Task<IResponse> CreateAsync(TEntity[] entities)
    {
        _dbSet.AddRange(entities);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }
    
    public async Task<ISingleResponse<TEntity>> GetByIdAsync(
        TKey1 key1, TKey2 key2,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (includes is not null && includes.Length > 0)
            foreach (var include in includes)
                query  = query.Include(include);
                
        var entity = await query.SingleOrDefaultAsync(x => x.Key1!.Equals(key1) && x.Key2!.Equals(key2));

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);
        
        return Response.Success(entity);
    }
    
    public async Task<ICollectionResponse<TEntity>> GetAllAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (includes is not null && includes.Any())
            foreach (var include in includes)
                query = query.Include(include);

        if (filter is not null)
            query = query.Where(filter);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (entities is null || entities.Count == 0)
            return Response.FailureCollection<TEntity>(Error.TEntityNotFound);

        return Response.Success(entities, totalItems, totalPages, page, pageSize);
    }
    
    public async Task<IResponse> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }

    public async Task<IResponse> UpdateAsync(TEntity[] entities)
    {
        _dbSet.UpdateRange(entities);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }
    
    public async Task<IResponse> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
