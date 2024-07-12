namespace ICorteApi.Presentation.Exceptions;

public class UserValidationException(Error[] errors)
    : BadRequestException("User validation failed", errors)
{
}
