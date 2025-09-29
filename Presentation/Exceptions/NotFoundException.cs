using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class NotFoundException<TEntity>(
    ILogger<IService<TEntity>> logger,
    string message,
    params Error[]? errors)
    : BaseException<TEntity>(logger, message, errors)
        where TEntity : class, IBaseTableEntity
{
    public override int HttpStatusCode => StatusCodes.Status404NotFound;
    public override string Title => GetTitle(this);
}
