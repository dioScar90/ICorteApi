using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRepository<TEntity> : IRepository where TEntity : class, IBaseTableEntity
{
}

public interface IRepository
{
}
