using ICorteApi.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity, TDtoResponse, TDtoRequest>(
    AppDbContext context,
    UserService userService)
    : IBaseService<TEntity, TDtoResponse, TDtoRequest>
        where TEntity : class, IBaseTableEntity<TEntity, TDtoResponse, TDtoRequest>
        where TDtoResponse : class, IDtoResponse<TEntity>
        where TDtoRequest : class, IDtoRequest<TEntity>
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    protected readonly UserService _userService = userService;
    
    protected async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    protected static async Task CommitAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();
    protected static async Task RollbackAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();

    protected async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public abstract Task<TDtoResponse> CreateAsync(TDtoRequest dto);
    
    public virtual async Task<PaginationResponse<TDtoResponse>> GetAllAsync(
        PaginationProperties<TEntity, TDtoResponse> props)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

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
    
    public async Task<bool> UpdateAsync(TEntity entity, TDtoRequest dto)
    {
        entity.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await SaveChangesAsync();
    }
}

