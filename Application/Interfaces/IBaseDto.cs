using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IDtoResponse<TEntity> where TEntity : class, IBaseTableEntity { }
public interface IDtoRequest<TEntity> where TEntity : class, IBaseTableEntity { }
