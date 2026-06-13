using ICorteApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity>(AppDbContext context) : IService<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly DbSet<TEntity> dbSet = context.Set<TEntity>();
    
    protected async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await context.Database.BeginTransactionAsync(cancellationToken);
    protected async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken) > 0;
    
    public virtual async Task<PaginationResponse<TDtoResponse>> GetAllAsync<TDtoResponse>(
        PaginationProperties<TEntity, TDtoResponse> props,
        CancellationToken cancellationToken = default)
            where TDtoResponse : class, IDtoResponse<TEntity>
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<TEntity> query = dbSet.AsNoTracking();

        foreach (var inlcude in props.Includes)
        {
            query = query.Include(inlcude);
        }

        query = query.Where(props.Filter);

        query = props.OrderBy.IsDesc
            ? query.OrderByDescending(props.OrderBy.KeySelector)
            : query.OrderBy(props.OrderBy.KeySelector);

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)props.PageSize);

        int page = props.Page > 0 && totalPages > 0 ? Math.Clamp(props.Page, 1, totalPages) : 0;

        var entities = totalItems == 0 ? [] : await query
            .Skip((page - 1) * props.PageSize)
            .Take(props.PageSize)
            .Select(props.Select)
            .ToArrayAsync(cancellationToken);

        return new(entities ?? [], totalItems, totalPages, page, props.PageSize);
    }
}

