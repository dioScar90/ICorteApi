using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBaseCompositeKeyService<TEntity, TKey1, TKey2> : IBaseService<TEntity>
    where TEntity : class, ICompositeKeyEntity<TKey1, TKey2>, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> GetByIdAsync(TKey1 id1, TKey2 id2);

    Task<IResponse> UpdateAsync(TKey1 id1, TKey2 id2, IDtoRequest<TEntity> dto);

    Task<IResponse> DeleteAsync(TKey1 id1, TKey2 id2);
}
