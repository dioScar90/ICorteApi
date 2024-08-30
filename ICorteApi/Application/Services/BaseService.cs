using System.Linq.Expressions;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity>(IBaseRepository<TEntity> repository) : IService<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly IBaseRepository<TEntity> _repository = repository;

    protected async Task<TEntity?> CreateAsync(TEntity entity)
    {
        return await _repository.CreateAsync(entity);
    }

    protected async Task<TEntity?> GetByIdAsync(
        params object[] primaryKeys)
    {
        return await _repository.GetByIdAsync(primaryKeys);
    }

    protected async Task<TEntity?> GetByIdAsync(
        Expression<Func<TEntity, bool>> filterId,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return await _repository.GetByIdAsync(filterId, includes);
    }

    protected async Task<TEntity[]> GetAllAsync(PaginationProperties<TEntity> props)
    {
        return await _repository.GetAllAsync(props);
    }

    protected async Task<bool> UpdateAsync(TEntity entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    protected async Task<bool> DeleteAsync(TEntity entity)
    {
        return await _repository.DeleteAsync(entity);
    }
}
