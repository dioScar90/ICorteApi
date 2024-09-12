using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IService<TEntity> : IService where TEntity : class, IBaseTableEntity
{
}

public interface IService
{
}
