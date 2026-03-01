using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public interface IBaseErrors<TEntity> : IBaseErrors
    where TEntity : class, IBaseTableEntity
{
    BadRequest<Error> BadRequest(params Error[] errors);
    BadRequest<Error> Create(params Error[] errors);
    BadRequest<Error> Update(params Error[] errors);
    BadRequest<Error> Delete(params Error[] errors);
    NotFound<Error> NotFound(params Error[] errors);
}

public interface IBaseErrors
{
}
