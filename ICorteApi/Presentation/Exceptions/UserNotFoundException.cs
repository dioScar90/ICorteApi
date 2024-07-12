namespace ICorteApi.Presentation.Exceptions;

public sealed class UserNotFoundException(int userId)
    : NotFoundException($"The user with the id {userId} was not found")
{
}
