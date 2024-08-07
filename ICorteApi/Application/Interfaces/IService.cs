using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IService<TEntity> where TEntity : class, IBaseTableEntity
{
}
