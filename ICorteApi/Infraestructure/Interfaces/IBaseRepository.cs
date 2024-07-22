using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepository<TEntity, TKey> where TEntity : class, IBaseTableEntity, IBaseEntity
{
    Task<bool> SaveChangesAsync();
    Task<IResponse> CreateAsync(TEntity entity);
    Task<IResponse> CreateManyAsync(TEntity[] entities);
    Task<ISingleResponse<TEntity>> GetByIdAsync(TKey id);
    Task<ICollectionResponse<TEntity>> GetAllAsync(int page, int pageSize, Func<TEntity, bool>? filter = null);
    Task<IResponse> UpdateAsync(TEntity entity);
    Task<IResponse> UpdateManyAsync(TEntity[] entities);
    Task<IResponse> DeleteAsync(TEntity entity);
}
