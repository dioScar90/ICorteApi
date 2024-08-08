using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public abstract class BaseService<TEntity>(IBaseRepository<TEntity> repository) : IBaseService<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly IBaseRepository<TEntity> _repository = repository;
    
    public async Task<ISingleResponse<TEntity>> CreateByEntityAsync(TEntity entity)
    {
        return await _repository.CreateAsync(entity);
    }
    
    public async Task<ICollectionResponse<TEntity>> GetAllAsync(int? pageAux, int? pageSizeAux)
    {
        int page = pageAux is null or <= 0 ? 1 : (int)pageAux;
        int pageSize = pageSizeAux is null or <= 0 ? 1 : (int)pageSizeAux;
        
        return await _repository.GetAllAsync(page, pageSize);
    }
}
