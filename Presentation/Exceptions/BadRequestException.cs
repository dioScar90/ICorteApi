using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public class BadRequestException<TEntity>(
    ILogger<IService<TEntity>> logger,
    string message,
    params Error[]? errors)
    : BaseException<TEntity>(logger, message, errors)
        where TEntity : class, IBaseTableEntity
{
    public override int HttpStatusCode => StatusCodes.Status400BadRequest;
    public override string Title => GetTitle(this);
}
