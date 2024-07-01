using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBaseRepository
{
    public Task<IResponseModel> CreateAsync<T>(T entity) where T : class;
    public void UpdateAsync<T>(T entity) where T : class;
    public void DeleteAsync<T>(T entity) where T : class;
    Task<bool> SaveChangesAsync();
}
