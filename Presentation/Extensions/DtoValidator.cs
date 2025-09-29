using FluentValidation;
using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Extensions;

public static class Validator
{
    public static void ThrowExceptionIfInvalid<TEntity, TDtoRequest>(
        this TDtoRequest dto,
        IValidator<TDtoRequest> validator,
        IBaseErrors<TEntity> entityErrors,
        ILogger<IService<TEntity>> logger)
            where TEntity : class, IBaseTableEntity
            where TDtoRequest : class, IDtoRequest<TEntity>
    {
        var results = validator.Validate(dto);
        
        if (!results.IsValid)
        {
            var errors = results.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
                .ToArray();

            logger.LogWarning("Não...");
            entityErrors.ThrowValidationException(errors);
        }
    }
}
