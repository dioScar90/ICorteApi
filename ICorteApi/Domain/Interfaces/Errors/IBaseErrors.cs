using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IBaseErrors<TEntity> where TEntity : class, IBaseTableEntity
{
    void ThrowCreateException(params Error[]? errors);
    void ThrowUpdateException(params Error[]? errors);
    void ThrowDeleteException(params Error[]? errors);
    void ThrowNotFoundException(params Error[]? errors);
    void ThrowValidationException(params Error[]? errors);
    void ThrowBadRequestException(params Error[]? errors);
}
