using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public abstract class BasePrimaryKeyService<TEntity, TKey>(IBasePrimaryKeyRepository<TEntity, TKey> repository)
    : BaseService<TEntity>(repository), IBasePrimaryKeyService<TEntity, TKey>
    where TEntity : class, IPrimaryKeyEntity<TKey>, IBaseTableEntity
    where TKey : IEquatable<TKey>
{
    protected new readonly IBasePrimaryKeyRepository<TEntity, TKey> _repository = repository;

    public async Task<ISingleResponse<TEntity>> GetByIdAsync(TKey id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IResponse> UpdateAsync(IDtoRequest<TEntity> dto, TKey id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);

        return await _repository.UpdateAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(TKey id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await _repository.DeleteAsync(entity);
    }
}
