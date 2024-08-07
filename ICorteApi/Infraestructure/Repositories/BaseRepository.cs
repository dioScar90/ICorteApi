using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public abstract class BaseRepository<TEntity>(AppDbContext context) : IBaseRepository<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    protected async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public virtual async Task<ISingleResponse<TEntity>> CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return await SaveChangesAsync() ? Response.Success(entity) : Response.Failure<TEntity>(Error.CreateError);
    }

    public virtual async Task<ICollectionResponse<TEntity>> GetAllAsync(
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

    public virtual async Task<IResponse> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }

    public virtual async Task<IResponse> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
