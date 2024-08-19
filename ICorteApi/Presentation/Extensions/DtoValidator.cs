using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class DtoValidator
{
    public static void CheckAndThrowExceptionIfInvalid<TDto, TEntity>(
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
