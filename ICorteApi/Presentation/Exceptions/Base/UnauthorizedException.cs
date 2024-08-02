namespace ICorteApi.Presentation.Exceptions;

public abstract class UnauthorizedException(string message) : Exception(message)
{
}
