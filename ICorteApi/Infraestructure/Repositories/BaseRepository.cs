using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public abstract class BaseRepository<TEntity, TKey>(AppDbContext context, DbSet<TEntity> table) : IBaseRepository<TEntity, TKey>
    where TEntity : class, IBaseTableEntity, IBaseEntity
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<TEntity> _table = table;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    Task<bool> IBaseRepository<TEntity, TKey>.SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
    
    public async Task<IResponse> CreateAsync(TEntity entity)
    {
        _table.Add(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }
    
    public async Task<IResponse> CreateManyAsync(TEntity[] entities)
    {
        _table.AddRange(entities);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }

    public async Task<ISingleResponse<TEntity>> GetByIdAsync(TKey id)
    {
        var entity = await _table
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id));

        if (entity is null)
            return Response.Failure<TEntity>(Error.TEntityNotFound);

        return Response.Success(entity);
    }
    
    public async Task<ICollectionResponse<TEntity>> GetAllAsync(int page, int pageSize, Func<TEntity, bool>? filter = null)
    {
        IQueryable<TEntity> query = _table.AsNoTracking();

        if (filter is not null)
            _ = query.Where(filter);
        
        // Aplica paginação
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        if (!entities.Any())
            return Response.FailureCollection<TEntity>(Error.TEntityNotFound);

        return Response.Success(
            entities
            , totalItems, totalPages, page, pageSize
            )
        // {
        //     TotalItems = totalItems,
        //     TotalPages = totalPages,
        //     CurrentPage = page,
        //     PageSize = pageSize
        // }
        ;
        
        // var response = new CollectionResponse<TEntity>
        // {
        //     Data = entities,
        //     TotalItems = totalItems,
        //     TotalPages = totalPages,
        //     CurrentPage = page,
        //     PageSize = pageSize
        // };

        // return Response.Success(entities);
    }
    
    public async Task<IResponse> UpdateAsync(TEntity entity)
    {
        _table.Update(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }

    public async Task<IResponse> UpdateManyAsync(TEntity[] entities)
    {
        _table.UpdateRange(entities);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }
    
    public async Task<IResponse> DeleteAsync(TEntity entity)
    {
        _table.Remove(entity);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
