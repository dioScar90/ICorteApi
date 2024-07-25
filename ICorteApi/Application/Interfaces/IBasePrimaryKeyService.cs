using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBasePrimaryKeyService<TEntity, TKey> : IBaseService<TEntity>
    where TEntity : class, IPrimaryKeyEntity<TKey>, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> GetByIdAsync(TKey key);

    Task<IResponse> UpdateAsync(TKey key, IDtoRequest<TEntity> dto);

    Task<IResponse> DeleteAsync(TKey key);
}
