using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IBaseErrors<TEntity> where TEntity : class, IBaseTableEntity
{
    void ThrowCreateException();
    void ThrowUpdateException();
    void ThrowDeleteException();
    void ThrowNotFoundException();
    void ThrowValidationException(Error[] errors);
    void ThrowValidationException(IDictionary<string, string[]> errors);
    void ThrowBadRequestException(string? message = null);
}
