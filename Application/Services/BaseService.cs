using ICorteApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity>(AppDbContext context) : IService<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly DbSet<TEntity> dbSet = context.Set<TEntity>();
    
    protected async Task<IDbContextTransaction> BeginTransactionAsync() => await context.Database.BeginTransactionAsync();
    protected async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
    
    public virtual async Task<PaginationResponse<TDtoResponse>> GetAllAsync<TDtoResponse>(
        PaginationProperties<TEntity, TDtoResponse> props)
            where TDtoResponse : class, IDtoResponse<TEntity>
    {
        IQueryable<TEntity> query = dbSet.AsNoTracking();

        foreach (var inlcude in props.Includes)
        {
            query = query.Include(inlcude);
        }

        query = query.Where(props.Filter);

        query = props.OrderBy.IsDesc
            ? query.OrderByDescending(props.OrderBy.KeySelector)
            : query.OrderBy(props.OrderBy.KeySelector);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)props.PageSize);

        int page = props.Page > 0 && totalPages > 0 ? Math.Clamp(props.Page, 1, totalPages) : 0;

        var entities = totalItems == 0 ? [] : await query
            .Skip((page - 1) * props.PageSize)
            .Take(props.PageSize)
            .Select(props.Select)
            .ToArrayAsync();

        return new(entities ?? [], totalItems, totalPages, page, props.PageSize);
    }
}

