using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class UnauthorizedException(string message, params Error[]? errors) : BaseException(message, errors)
{
}
