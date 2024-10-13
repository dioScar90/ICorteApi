using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public class BadRequestException(string message, params Error[]? errors) : BaseException(message, errors)
{
}
