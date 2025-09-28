using FluentValidation;
using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Extensions;

public static class Validator
{
    public static void ThrowExceptionIfInvalid<TDto, TEntity>(
        this TDto dto, IValidator<TDto> validator, IBaseErrors<TEntity> entityErrors)
            where TEntity : class, IBaseTableEntity
            where TDto : IDtoRequest<TEntity>
    {
        var results = validator.Validate(dto);
        
        if (!results.IsValid)
        {
            var errors = results.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
                .ToArray();
            
            entityErrors.ThrowValidationException(errors);
        }
    }
}
