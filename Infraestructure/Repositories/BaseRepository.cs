using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Infraestructure.Repositories;

public abstract class BaseRepository<TEntity>(AppDbContext context) : IBaseRepository<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    protected async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    protected async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    protected static async Task CommitAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();
    protected static async Task RollbackAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();

    public virtual async Task<TEntity?> CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return await SaveChangesAsync() ? entity : null;
    }

    public virtual async Task<TEntity?> GetByIdAsync(params object[] primaryKeys)
    {
        return await _dbSet.FindAsync(primaryKeys);
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(
        Expression<Func<TEntity, bool>> filterId,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return await includes
            .Aggregate(
                (IQueryable<TEntity>)_dbSet,
                (current, include) => current.Include(include))
            .SingleOrDefaultAsync(filterId);
    }
    
    public virtual async Task<PaginationResponse<TEntity>> GetAllAsync(IPaginationProperties<TEntity> props)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        query = props.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = query.Where(props.Filter);
        query = props.IsDescending ? query.OrderByDescending(props.OrderBy) : query.OrderBy(props.OrderBy);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)props.PageSize);
        
        int page = props.Page > 0 && totalPages > 0 ? Math.Clamp(props.Page, 1, totalPages) : 0;
        
        if (totalItems == 0)
            return new([], totalItems, totalPages, page, props.PageSize);
        
        var entities = await query
            .Skip((page - 1) * props.PageSize)
            .Take(props.PageSize)
            .ToArrayAsync();
        
        return new(entities ?? [], totalItems, totalPages, page, props.PageSize);
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await SaveChangesAsync();
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await SaveChangesAsync();
    }
}

public record PaginationResponse<TEntity>(
    TEntity[] Items,
    int TotalItems,
    int TotalPages,
    int Page,
    int PageSize
);
