using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepositoryWithMultiplePrimaryKey<TEntity> where TEntity : class, IBaseTableEntity, IBaseCrudEntity
{
    Task<bool> SaveChangesAsync();
    Task<IResponse> CreateAsync(TEntity entity);
    Task<IResponse> CreateAsync(TEntity[] entities);
    Task<IResponse> CreateManyAsync(TEntity[] entities);
    Task<ISingleResponse<TEntity>> GetByIdAsync(int id);
    Task<ICollectionResponse<TEntity>> GetAllAsync(int page, int pageSize, Func<TEntity, bool>? filter = null);
    Task<IResponse> UpdateAsync(TEntity entity);
    Task<IResponse> UpdateManyAsync(TEntity[] entities);
    Task<IResponse> DeleteAsync(TEntity entity);
}
