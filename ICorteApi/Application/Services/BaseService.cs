using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity>(IBaseRepository<TEntity> repository) : IBaseService<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly IBaseRepository<TEntity> _repository = repository;
    
    public async Task<IResponse> CreateAsync(IDtoRequest<TEntity> dto)
    {
        var entity = dto.CreateEntity()!;
        return await _repository.CreateAsync(entity);
    }
    
    public async Task<IResponse> CreateAsync(IDtoRequest<TEntity>[] dtos)
    {
        var entities = dtos.Select(dto => dto.CreateEntity()!).ToArray();
        return await _repository.CreateAsync(entities);
    }
    
    public async Task<ICollectionResponse<TEntity>> GetAllAsync(
        int page,
        int pageSize)
    {
        return await _repository.GetAllAsync(1, 25);
        // IQueryable<TEntity> query = _dbSet.AsNoTracking();

        // if (includes is not null && includes.Any())
        //     foreach (var include in includes)
        //         query = query.Include(include);

        // if (filter is not null)
        //     query = query.Where(filter);

        // var totalItems = await query.CountAsync();
        // var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // var entities = await query
        //     .Skip((page - 1) * pageSize)
        //     .Take(pageSize)
        //     .ToListAsync();

        // if (entities is null || entities.Count == 0)
        //     return Response.FailureCollection<TEntity>(Error.TEntityNotFound);

        // return Response.Success(entities, totalItems, totalPages, page, pageSize);
    }
    
    public async Task<IResponse> UpdateEntityAsync(TEntity entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<IResponse> UpdateEntityAsync(TEntity[] entities)
    {
        return await _repository.UpdateAsync(entities);
    }
    
    public async Task<IResponse> DeleteEntityAsync(TEntity entity)
    {
        return await _repository.DeleteAsync(entity);
    }
}
