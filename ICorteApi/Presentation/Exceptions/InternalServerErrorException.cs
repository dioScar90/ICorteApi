namespace ICorteApi.Presentation.Exceptions;

public class InternalServerErrorException(string message) : Exception(message)
{
}
