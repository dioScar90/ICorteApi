using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class InternalServerErrorException(string message, params Error[]? errors) : BaseException(message, errors)
{
}
