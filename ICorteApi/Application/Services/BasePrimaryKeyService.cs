using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public abstract class BasePrimaryKeyService<TEntity, TKey>(IBasePrimaryKeyRepository<TEntity, TKey> baseRepository)
    : BaseService<TEntity>(baseRepository), IBasePrimaryKeyService<TEntity, TKey>
    where TEntity : class, IPrimaryKeyEntity<TKey>, IBaseTableEntity
{
    protected readonly IBasePrimaryKeyRepository<TEntity, TKey> _primaryKeyRepository = baseRepository;
    
    public async Task<ISingleResponse<TEntity>> GetByIdAsync(TKey key)
    {
        return await _primaryKeyRepository.GetByIdAsync(key);
    }

    public async Task<IResponse> UpdateAsync(TKey key, IDtoRequest<TEntity> dto)
    {
        var resp = await GetByIdAsync(key);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);
        
        return await UpdateEntityAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(TKey key, IDtoRequest<TEntity> dto)
    {
        var resp = await GetByIdAsync(key);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        return await DeleteEntityAsync(entity);
    }
}
