using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IDto<TEntity> where TEntity : class, IBaseTableEntity { }
