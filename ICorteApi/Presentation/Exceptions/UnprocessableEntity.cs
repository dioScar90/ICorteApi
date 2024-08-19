using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class UnprocessableEntity(string message, params Error[]? errors) : BaseException(message, errors)
{
}
