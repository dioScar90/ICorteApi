using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class UnauthorizedException<TEntity>(
    ILogger<IService<TEntity>> logger,
    string message,
    params Error[]? errors)
    : BaseException<TEntity>(logger, message, errors)
        where TEntity : class, IBaseTableEntity
{
    public override int HttpStatusCode => StatusCodes.Status401Unauthorized;
    public override string Title => GetTitle(this);
}
