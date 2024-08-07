using System.Linq.Expressions;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRepository<TEntity> where TEntity : class, IBaseTableEntity
{
}
