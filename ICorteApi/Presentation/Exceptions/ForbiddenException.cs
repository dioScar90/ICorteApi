using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class ForbiddenException(string message, params Error[]? errors) : BaseException(message, errors)
{
}
