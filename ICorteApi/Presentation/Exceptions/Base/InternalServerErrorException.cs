namespace ICorteApi.Presentation.Exceptions;

public abstract class InternalServerErrorException(string message) : Exception(message)
{
}
