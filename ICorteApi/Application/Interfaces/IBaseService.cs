using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBaseService<TEntity> : IService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(IDtoRequest<TEntity> dto);
    Task<ICollectionResponse<TEntity>> GetAllAsync(int page, int pageSize);
}
