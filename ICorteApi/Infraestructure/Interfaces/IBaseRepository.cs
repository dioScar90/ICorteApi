using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepository
{
    public Task<IResponseModel> CreateAsync<T>(T entity) where T : class, IBaseEntity;
    public void Update<T>(T entity) where T : class, IBaseEntity;
    public void Delete<T>(T entity) where T : class, IBaseEntity;
    Task<bool> SaveChangesAsync();
}
