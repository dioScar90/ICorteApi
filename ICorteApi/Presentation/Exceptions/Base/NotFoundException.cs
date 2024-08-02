namespace ICorteApi.Presentation.Exceptions;

public abstract class NotFoundException(string message) : Exception(message)
{
}
