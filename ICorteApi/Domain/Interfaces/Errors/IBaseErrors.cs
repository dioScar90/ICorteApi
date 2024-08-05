using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IBaseErrors<TEntity> where TEntity : class, IBaseTableEntity
{
    Error CreateError();
    Error UpdateError();
    Error DeleteError();
    Error NotFoundError();
}
