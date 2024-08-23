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

    public virtual async Task<ISingleResponse<TEntity>> GetByIdAsync(
        Expression<Func<TEntity, bool>> filterId,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        var entity = await query.SingleOrDefaultAsync(filterId);

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);

        return Response.Success(entity);
    }
    
    public virtual async Task<ICollectionResponse<TEntity>> GetAllAsync(IGetAllProperties<TEntity> props)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        
        query = props.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = query.Where(props.Filter);

        if (props.OrderBy is not null)
        {
            query = (bool)props.IsDescending! ? query.OrderByDescending(props.OrderBy) : query.OrderBy(props.OrderBy);
        }
        
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)props.PageSize);

        var entities = await query
            .Skip((props.Page - 1) * props.PageSize)
            .Take(props.PageSize)
            .ToListAsync();

        if (entities is not { Count: > 0 })
            return Response.FailureCollection<TEntity>(Error.TEntityNotFound);

        return Response.Success(entities, new(totalItems, totalPages, props.Page, props.PageSize));
    }
    
    public virtual async Task<IResponse> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }

    public virtual async Task<IResponse> DeleteAsync(TEntity entity)
    {
        if (entity is IBaseEntity<TEntity> baseEntity)
        {
            baseEntity.DeleteEntity();
            _dbSet.Update(entity); // Update because here it's soft delete
        }
        else
        {
            _dbSet.Remove(entity);
        }

        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
