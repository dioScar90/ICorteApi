using ICorteApi.Domain.Base;

namespace ICorteApi.Application.Services;

public interface IBaseService<TEntity, TDtoResponse, TDtoRequest> : IService<TEntity>
    where TEntity : class, IBaseTableEntity
    where TDtoResponse : class, IDtoResponse<TEntity>
    where TDtoRequest : class, IDtoRequest<TEntity>
{
    Task<TDtoResponse> CreateAsync(TDtoRequest dto);
    
    Task<PaginationResponse<TDtoResponse>> GetAllAsync(PaginationProperties<TEntity, TDtoResponse> props);

    Task<bool> UpdateAsync(TEntity entity, TDtoRequest dto);

    Task<bool> DeleteAsync(TEntity entity);
}
