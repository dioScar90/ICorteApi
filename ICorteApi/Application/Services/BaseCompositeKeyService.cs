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
    
    public virtual Task<ISingleResponse<TEntity>> GetByIdAsync(TKey1 id1, TKey2 id2)
    {
        throw new NotImplementedException();
    }
    
    public virtual Task<IResponse> UpdateAsync(TKey1 id1, TKey2 id2, IDtoRequest<TEntity> dto)
    {
        // var resp = await GetByIdAsync(id1, id2);

        // if (!resp.IsSuccess)
        //     return resp;
        
        // var entity = resp.Value!;
        // entity.UpdateEntityByDto(dto);
        
        // return await UpdateEntityAsync(entity);
        throw new NotImplementedException();
    }

    public virtual Task<IResponse> DeleteAsync(TKey1 id1, TKey2 id2)
    {
        // var resp = await GetByIdAsync(id1, id2);

        // if (!resp.IsSuccess)
        //     return resp;
        
        // var entity = resp.Value!;
        // return await DeleteEntityAsync(entity);
        throw new NotImplementedException();
    }
}
