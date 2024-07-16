// using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class UserValidationException(Error[] errors)
    : BadRequestException("User validation failed", errors)
{
}
