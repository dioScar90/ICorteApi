using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public abstract class BaseCompositeKeyService<TEntity, TKey1, TKey2>(IBaseCompositeKeyRepository<TEntity, TKey1, TKey2> baseRepository)
    : BaseService<TEntity>(baseRepository), IBaseCompositeKeyService<TEntity, TKey1, TKey2>
    where TEntity : class, ICompositeKeyEntity<TKey1, TKey2>, IBaseTableEntity
{
    protected readonly IBaseCompositeKeyRepository<TEntity, TKey1, TKey2> _compositeKeyRepository = baseRepository;
    
    public async Task<ISingleResponse<TEntity>> GetByIdAsync(TKey1 key1, TKey2 key2)
    {
        return await _compositeKeyRepository.GetByIdAsync(key1, key2);
    }
    
    public async Task<IResponse> UpdateAsync(TKey1 key1, TKey2 key2, IDtoRequest<TEntity> dto)
    {
        var resp = await GetByIdAsync(key1, key2);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);
        
        return await UpdateEntityAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(TKey1 key1, TKey2 key2)
    {
        var resp = await GetByIdAsync(key1, key2);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        return await DeleteEntityAsync(entity);
    }
}
