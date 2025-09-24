using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity, TDto>(AppDbContext context) : IBaseService<TEntity, TDto>
    where TEntity : class, IBaseTableEntity
    where TDto : class, IDto<TEntity>
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
    
    private void VerifyPrimaryKeys(params object[] primaryKeys)
    {
        // Obtém metadados do modelo do EF
        var entityType = _context.Model.FindEntityType(typeof(TEntity))
            ?? throw new InvalidOperationException($"Entidade {typeof(TEntity).Name} não encontrada no modelo.");

        var pk = entityType.FindPrimaryKey()
            ?? throw new InvalidOperationException($"Entidade {typeof(TEntity).Name} não possui chave primária.");

        // Valida quantidade de chaves
        if (pk.Properties.Count != primaryKeys.Length)
            throw new ArgumentException(
                $"Esperado {pk.Properties.Count} valores de chave, mas foram recebidos {primaryKeys.Length}.");

        // Valida tipos em runtime
        for (int i = 0; i < pk.Properties.Count; i++)
        {
            var expectedType = pk.Properties[i].ClrType;
            var received = primaryKeys[i];

            if (received == null || !expectedType.IsAssignableFrom(received.GetType()))
            {
                throw new ArgumentException(
                    $"Tipo inválido para chave '{pk.Properties[i].Name}'. Esperado {expectedType.Name}.");
            }
        }
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(params object[] primaryKeys)
    {
        VerifyPrimaryKeys(primaryKeys);
        return await _dbSet.FindAsync(primaryKeys);
    }
    
    public virtual async Task<TDto?> GetByIdAsync(
        Expression<Func<TEntity, bool>> filterId,
        Expression<Func<TEntity, TDto>> selector,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> suamae = _dbSet
            .Where(filterId);

        foreach (var include in includes)
            suamae.Include(include);

        return await suamae.Select(selector).FirstAsync();

        // return await suamae.SingleOrDefaultAsync(filterId);
    }
    
    public virtual async Task<PaginationResponse<TEntity>> GetAllAsync(PaginationProperties<TEntity> props)
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

