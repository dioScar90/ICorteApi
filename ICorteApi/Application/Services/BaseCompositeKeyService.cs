using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public abstract class BaseCompositeKeyService<TEntity, TKey1, TKey2>(IBaseCompositeKeyRepository<TEntity, TKey1, TKey2> repository)
    : BaseService<TEntity>(repository), IBaseCompositeKeyService<TEntity, TKey1, TKey2>
    where TEntity : class, ICompositeKeyEntity<TKey1, TKey2>, IBaseTableEntity
{
    protected new readonly IBaseCompositeKeyRepository<TEntity, TKey1, TKey2> _repository = repository;
    
    public virtual Task<ISingleResponse<TEntity>> GetByIdAsync(TKey1 id1, TKey2 id2)
    {
        throw new NotImplementedException();
    }
    
    public virtual Task<IResponse> UpdateAsync(TKey1 id1, TKey2 id2, IDtoRequest<TEntity> dto)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IResponse> DeleteAsync(TKey1 id1, TKey2 id2)
    {
        throw new NotImplementedException();
    }
}
