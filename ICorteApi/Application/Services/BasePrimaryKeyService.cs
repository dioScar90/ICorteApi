using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public abstract class BasePrimaryKeyService<TEntity, TKey>(IBasePrimaryKeyRepository<TEntity, TKey> repository)
    : BaseService<TEntity>(repository), IBasePrimaryKeyService<TEntity, TKey>
    where TEntity : class, IPrimaryKeyEntity<TEntity, TKey>, IBaseTableEntity
    where TKey : IEquatable<TKey>
{
    protected new readonly IBasePrimaryKeyRepository<TEntity, TKey> _repository = repository;
    
    public virtual async Task<ISingleResponse<TEntity>> GetByIdAsync(TKey id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<IResponse> UpdateAsync(IDtoRequest<TEntity> dtoRequest, TKey id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;
        
        resp.Value!.UpdateEntityByDto(dtoRequest);
        return await _repository.UpdateAsync(resp.Value!);
    }

    public virtual async Task<IResponse> DeleteAsync(TKey id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;
        
        return await _repository.DeleteAsync(resp.Value!);
    }
}
